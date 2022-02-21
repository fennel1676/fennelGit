// MotionControlDll.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"

#include "MotionControlDll.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif



int g_nPort = 0;
int g_nTimeout = 0;
HANDLE				g_hComm;			// 통신 포트 파일 핸들
OVERLAPPED			g_osRead;			// 포트 파일 Overlapped structure
OVERLAPPED			g_osWrite;			// 포트 파일 Overlapped structure



unsigned short int	g_crc16_tbl[256] = {
	0x0000, 0xc0c1, 0xc181, 0x0140, 0xc301, 0x03c0, 0x0280, 0xc241,		0xc601, 0x06c0, 0x0780, 0xc741, 0x0500, 0xc5c1, 0xc481, 0x0440,
	0xcc01, 0x0cc0, 0x0d80, 0xcd41, 0x0f00, 0xcfc1, 0xce81, 0x0e40,		0x0a00, 0xcac1, 0xcb81, 0x0b40, 0xc901, 0x09c0, 0x0880, 0xc841,
	0xd801, 0x18c0, 0x1980, 0xd941, 0x1b00, 0xdbc1, 0xda81, 0x1a40,		0x1e00, 0xdec1, 0xdf81, 0x1f40, 0xdd01, 0x1dc0, 0x1c80, 0xdc41,
	0x1400, 0xd4c1, 0xd581, 0x1540, 0xd701, 0x17c0, 0x1680, 0xd641,		0xd201, 0x12c0, 0x1380, 0xd341, 0x1100, 0xd1c1, 0xd081, 0x1040,
	0xf001, 0x30c0, 0x3180, 0xf141, 0x3300, 0xf3c1, 0xf281, 0x3240,		0x3600, 0xf6c1, 0xf781, 0x3740, 0xf501, 0x35c0, 0x3480, 0xf441,
	0x3c00, 0xfcc1, 0xfd81, 0x3d40, 0xff01, 0x3fc0, 0x3e80, 0xfe41,		0xfa01, 0x3ac0, 0x3b80, 0xfb41, 0x3900, 0xf9c1, 0xf881, 0x3840,
	0x2800, 0xe8c1, 0xe981, 0x2940, 0xeb01, 0x2bc0, 0x2a80, 0xea41,		0xee01, 0x2ec0, 0x2f80, 0xef41, 0x2d00, 0xedc1, 0xec81, 0x2c40,
	0xe401, 0x24c0, 0x2580, 0xe541, 0x2700, 0xe7c1, 0xe681, 0x2640,		0x2200, 0xe2c1, 0xe381, 0x2340, 0xe101, 0x21c0, 0x2080, 0xe041,
	0xa001, 0x60c0, 0x6180, 0xa141, 0x6300, 0xa3c1, 0xa281, 0x6240,		0x6600, 0xa6c1, 0xa781, 0x6740, 0xa501, 0x65c0, 0x6480, 0xa441,
	0x6c00, 0xacc1, 0xad81, 0x6d40, 0xaf01, 0x6fc0, 0x6e80, 0xae41,		0xaa01, 0x6ac0, 0x6b80, 0xab41, 0x6900, 0xa9c1, 0xa881, 0x6840,
	0x7800, 0xb8c1, 0xb981, 0x7940, 0xbb01, 0x7bc0, 0x7a80, 0xba41,		0xbe01, 0x7ec0, 0x7f80, 0xbf41, 0x7d00, 0xbdc1, 0xbc81, 0x7c40,
	0xb401, 0x74c0, 0x7580, 0xb541, 0x7700, 0xb7c1, 0xb681, 0x7640,		0x7200, 0xb2c1, 0xb381, 0x7340, 0xb101, 0x71c0, 0x7080, 0xb041,
	0x5000, 0x90c1, 0x9181, 0x5140, 0x9301, 0x53c0, 0x5280, 0x9241,		0x9601, 0x56c0, 0x5780, 0x9741, 0x5500, 0x95c1, 0x9481, 0x5440,
	0x9c01, 0x5cc0, 0x5d80, 0x9d41, 0x5f00, 0x9fc1, 0x9e81, 0x5e40,		0x5a00, 0x9ac1, 0x9b81, 0x5b40, 0x9901, 0x59c0, 0x5880, 0x9841,
	0x8801, 0x48c0, 0x4980, 0x8941, 0x4b00, 0x8bc1, 0x8a81, 0x4a40,		0x4e00, 0x8ec1, 0x8f81, 0x4f40, 0x8d01, 0x4dc0, 0x4c80, 0x8c41,
	0x4400, 0x84c1, 0x8581, 0x4540, 0x8701, 0x47c0, 0x4680, 0x8641,		0x8201, 0x42c0, 0x4380, 0x8341, 0x4100, 0x81c1, 0x8081, 0x4040
};

BOOL	TickCheck(DWORD dPeriode, DWORD dLastTime)
{
	DWORD	dDef, dCurTime;
	dCurTime = (DWORD)GetCurrentTime();
	if (dLastTime > dCurTime)
		dDef = ((DWORD)0xffffffffL - dLastTime) + dCurTime;
	else
		dDef = dCurTime - dLastTime;
	return (dDef >= dPeriode);
}

unsigned short int	MakeCRC16(char *stxt, int ssiz)
{
	int	i;
	unsigned short int	iax, ibx, idx, ial, idh;

	iax = 0xFFFF;
	for (i = 0; i < ssiz; i++)
	{
		idx = ((unsigned short int)*stxt++ & 0x00FF) ^ iax;
		ibx = idx & 0x00FF;
		iax = g_crc16_tbl[ibx];
		ial = iax & 0x00FF;
		idh = (idx >> 8) & 0x00FF;
		ial = (ial ^ idh) & 0x00FF;
		iax = (iax & 0xFF00) | ial;
	}
	return (unsigned short int)iax;
}
unsigned short int	MakeCRC16(BYTE *stxt, int ssiz)
{
	int	i;
	unsigned short int	iax, ibx, idx, ial, idh;

	iax = 0xFFFF;
	for (i = 0; i < ssiz; i++)
	{
		idx = ((unsigned short int)*stxt++ & 0x00FF) ^ iax;
		ibx = idx & 0x00FF;
		iax = g_crc16_tbl[ibx];
		ial = iax & 0x00FF;
		idh = (idx >> 8) & 0x00FF;
		ial = (ial ^ idh) & 0x00FF;
		iax = (iax & 0xFF00) | ial;
	}
	return (unsigned short int)iax;
}

BOOL	ComOpen(int nPort)
{
	// Local 변수.
	COMMTIMEOUTS		timeouts;
	DCB				dcb;
	//int nBaud = 115200;

	// overlapped structure 변수 초기화.
	g_osRead.Offset = 0;
	g_osRead.OffsetHigh = 0;
	//--> Read 이벤트 생성에 실패..
	if (!(g_osRead.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL)))
	{
		return FALSE;
	}

	g_osWrite.Offset = 0;
	g_osWrite.OffsetHigh = 0;
	//--> Write 이벤트 생성에 실패..
	if (!(g_osWrite.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL)))
	{
		return FALSE;
	}

	//--> 포트명 저장..
	CString strPortName;
	strPortName.Format(_T("\\\\.\\COM%d"), nPort + 1);

	//--> 실제적인...RS 232 포트 열기..
	g_hComm = CreateFile(strPortName,
		GENERIC_READ | GENERIC_WRITE, 0, NULL,
		OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED,
		NULL);
	//--> 포트 열기에 실해하면..
	if (g_hComm == (HANDLE)-1)
		return FALSE;

	//===== 포트 상태 설정. =====
	// EV_RXCHAR event 설정...데이터가 들어오면.. 수신 이벤트가 발생하게끔..
	SetCommMask(g_hComm, EV_RXCHAR);
	// InQueue, OutQueue 크기 설정.
	SetupComm(g_hComm, BUFF_SIZE, BUFF_SIZE);
	// 포트 비우기.
	PurgeComm(g_hComm, PURGE_TXABORT | PURGE_TXCLEAR | PURGE_RXABORT | PURGE_RXCLEAR);
	// timeout 설정.
	timeouts.ReadIntervalTimeout = 0xFFFFFFFF;
	timeouts.ReadTotalTimeoutMultiplier = 0;
	timeouts.ReadTotalTimeoutConstant = 0;
	timeouts.WriteTotalTimeoutMultiplier = 2 * CBR_9600 / 115200;
	timeouts.WriteTotalTimeoutConstant = 0;

	SetCommTimeouts(g_hComm, &timeouts);
	// dcb 설정.... 포트의 실제적인..제어를 담당하는 DCB 구조체값 셋팅..
	dcb.DCBlength = sizeof(DCB);
	//--> 현재 설정된 값 중에서..
	GetCommState(g_hComm, &dcb);
	//--> 보드레이트를 바꾸고..
	dcb.BaudRate = 115200;
	//--> Data 8 Bit
	dcb.ByteSize = 8;						// 이부분의 설정값이 변경이 필요하게 되면 
											//--> Noparity						//
	dcb.Parity = NOPARITY;				// 함수의 인자를 받아서 처리하게끔 한다.
										//--> 1 Stop Bit					   	//
										//dcb.StopBits = ONESTOPBIT;				// 현재 버젼은 Com Port, Baudrate 부분만 받게 처리 한다.
	dcb.StopBits = ONESTOPBIT;				// 현재 버젼은 Com Port, Baudrate 부분만 받게 처리 한다.

											//--> 포트를 재..설정값으로.. 설정해보고..
	if (!SetCommState(g_hComm, &dcb))
	{
		return FALSE;
	}
	return TRUE;
}
BOOL	ComClose()
{
	BOOL bResult = TRUE;
	if (g_hComm != (HANDLE)-1)
	{
		//--> 마스크 해제..
		SetCommMask(g_hComm, 0);
		//--> 포트 비우기.
		PurgeComm(g_hComm, PURGE_TXABORT | PURGE_TXCLEAR | PURGE_RXABORT | PURGE_RXCLEAR);
		//--> 핸들 닫기
		BOOL bResult = CloseHandle(g_hComm);
		g_hComm = (HANDLE)-1;
	}


	return bResult;
}

int		ComRead(char* pBuf, int nSize)
{
	DWORD	dwRead, dwError, dwErrorFlags;
	COMSTAT	comstat;
	//--- system queue에 도착한 byte수만 미리 읽는다.
	ClearCommError(g_hComm, &dwErrorFlags, &comstat);
	//--> 시스템 큐에서 읽을 거리가 있으면..
	dwRead = comstat.cbInQue;
	if (dwRead > 0)
	{
		if (dwRead>(DWORD)nSize)
			dwRead = nSize;

		//--> 버퍼에 일단 읽어들이는데.. 만일..읽어들인값이 없다면..
		if (!ReadFile(g_hComm, pBuf, dwRead, &dwRead, &g_osRead))
		{
			//--> 읽을 거리가 남았으면..
			if (GetLastError() == ERROR_IO_PENDING)
			{
				//--------- timeouts에 정해준 시간만큼 기다려준다.
				while (!GetOverlappedResult(g_hComm, &g_osRead, &dwRead, TRUE))
				{
					dwError = GetLastError();
					if (dwError != ERROR_IO_INCOMPLETE)
					{
						ClearCommError(g_hComm, &dwErrorFlags, &comstat);
						break;
					}
				}
			}
			else
			{
				dwRead = 0;
				ClearCommError(g_hComm, &dwErrorFlags, &comstat);
			}
		}
	}
	//--> 실제 읽어들인 갯수를 리턴.
	return (int)dwRead;
}
int		ComWrite(char* pBuf, int nSize)
{
	DWORD	dwWritten, dwError, dwErrorFlags;
	COMSTAT	comstat;

	//--> 인자로 들어온 버퍼의 내용을 nToWrite 만큼 쓰고.. 쓴 갯수를.,dwWrite 에 넘김.
	if (!WriteFile(g_hComm, pBuf, nSize, &dwWritten, &g_osWrite))
	{
		//--> 아직 전송할 문자가 남았을 경우..
		if (GetLastError() == ERROR_IO_PENDING)
		{
			// 읽을 문자가 남아 있거나 전송할 문자가 남아 있을 경우 Overapped IO의
			// 특성에 따라 ERROR_IO_PENDING 에러 메시지가 전달된다.
			//timeouts에 정해준 시간만큼 기다려준다.
			while (!GetOverlappedResult(g_hComm, &g_osWrite, &dwWritten, TRUE))
			{
				dwError = GetLastError();
				if (dwError != ERROR_IO_INCOMPLETE)
				{
					ClearCommError(g_hComm, &dwErrorFlags, &comstat);
					break;
				}
			}
		}
		else
		{
			dwWritten = 0;
			ClearCommError(g_hComm, &dwErrorFlags, &comstat);
		}
	}
	//--> 실제 포트로 쓰여진 갯수를 리턴..
	return (int)dwWritten;
}

BOOL	ComOnline()
{
	return ((g_hComm == NULL) || (g_hComm == (HANDLE)-1)) ? FALSE : TRUE;
}
BOOL	ComState()
{
	DWORD	dwRead, dwErrorFlags;
	COMSTAT	comstat;
	//--- system queue에 도착한 byte수만 미리 읽는다.
	ClearCommError(g_hComm, &dwErrorFlags, &comstat);
	//--> 시스템 큐에서 읽을 거리가 있으면..
	dwRead = comstat.cbInQue;
	return	dwRead;
}

int		ComGetc(char* pBuf)
{
	return	ComRead(pBuf, 1);
}
int		ComPutc(char chBuf)
{
	return	ComWrite((char *)&chBuf, 1);
}


// Initial
//DllExportExternC	BOOL			MotionControlInitial(int nPort, int nTimeout)
//{
//	g_nPort = nPort;
//	g_nTimeout = nTimeout;
//
//	return ComOpen(g_nPort);
//}
//// Destroy
//DllExportExternC	BOOL			MotionControlDestroy()
//{
//	return ComClose();
//}

DllExportExternC	BOOL			ComInitial(int nPort, int nTimeout)
{
	g_nPort = nPort;
	g_nTimeout = nTimeout;

	return ComOpen(g_nPort);
}
// Destroy
DllExportExternC	BOOL			ComDestroy()
{
	return ComClose();
}

DllExportExternC	int			ComEQModeRead()
{
	//BOOL bResult = FALSE;

	int nMainCmd = 0;
	int nSubCmd = 10052;
	char szComDataFrame[128] = { 0 };
	int nDataLength = 4; //fixed
	int nDataBuf[32] = { 0 };

	short int nCRC = 0;
	int nRxCnt = 0;
	unsigned char chResp = 1;
	char ch = 0;

	int nResult = STATE_DISCONNECT;

	//stShmData.mDeviceAlarm = STATE_DISCONNECT;
	/* send message length = 23 byte
	[0]~[3]		: header
	[4]~[7]		: main cmd
	[8]~[11]	: sub cmd
	[12]		: response
	[13]~[16]	: data length
	[17]~[20]	: device
	[21]~[22]	: CRC
	*/

	// Header
	szComDataFrame[0] = (unsigned char)0xFF;
	szComDataFrame[1] = (unsigned char)0x55;
	szComDataFrame[2] = (unsigned char)0xEE;
	szComDataFrame[3] = (unsigned char)0xAA;
	// Data
	memmove(szComDataFrame + 4, &nMainCmd, 4);
	memmove(szComDataFrame + 8, &nSubCmd, 4);
	memmove(szComDataFrame + 12, &chResp, 1);
	memmove(szComDataFrame + 13, &nDataLength, 4);


	nCRC = MakeCRC16((char*)szComDataFrame, 17);
	memmove(szComDataFrame + 17, &nCRC, 4);


	// CRC-16
	nCRC = MakeCRC16((char*)szComDataFrame, READ_TX_SIZE - 2);
	memmove(szComDataFrame + (READ_TX_SIZE - 2), &nCRC, 2);


	//공유메모리에 write
	int nTxCnt = ComWrite(szComDataFrame, READ_TX_SIZE);

	BYTE szRxBuf[1024] = { 0 };

	if (READ_TX_SIZE == nTxCnt)
	{
		long timer = GetCurrentTime();

		while (nResult == STATE_DISCONNECT)
		{
			if (TickCheck(g_nTimeout, timer))
			{
				nResult = STATE_TIMEOUT;
				break;
			}

			if (ComGetc(&ch) <= 0)
				continue;

			switch (nRxCnt)
			{
			case 0: if ((unsigned char)ch == (unsigned char)0xFF) szRxBuf[nRxCnt++] = ch;	break;
			case 1: if ((unsigned char)ch == (unsigned char)0x55) szRxBuf[nRxCnt++] = ch;	break;
			case 2:	if ((unsigned char)ch == (unsigned char)0xEE) szRxBuf[nRxCnt++] = ch;	break;
			case 3:	if ((unsigned char)ch == (unsigned char)0xAA) szRxBuf[nRxCnt++] = ch;	break;
			default:
			{
				szRxBuf[nRxCnt++] = ch;

				if (nRxCnt >= READ_RX_SIZE)
				{
					nResult = STATE_CRC_ERR;

					memmove(&nCRC, &szRxBuf[nRxCnt - 2], 2);
					short int nCheckCRC = 0;
					nCheckCRC = MakeCRC16(szRxBuf, nRxCnt - 2);
					if (nCRC == nCheckCRC)
					{
						memmove(&nResult, &szRxBuf[17], 4);//data length : 16byte id, pos, di, do


						//nAlarm = (pDeviceData->mDeviceID != nID) ? STATE_ID_ERR : STATE_SUCCESS;
					}					
				}
			}break;
			}
		}//end while
	}

	return nResult;
}


DllExportExternC	int			ComEQConv1RunRpmRead()
{
	//BOOL bResult = FALSE;

	int nMainCmd = 0;
	int nSubCmd = 10102;
	char szComDataFrame[128] = { 0 };
	int nDataLength = 4; //fixed
	int nDataBuf[32] = { 0 };

	short int nCRC = 0;
	int nRxCnt = 0;
	unsigned char chResp = 1;
	char ch = 0;

	int nResult = STATE_DISCONNECT;

	//stShmData.mDeviceAlarm = STATE_DISCONNECT;
	/* send message length = 23 byte
	[0]~[3]		: header
	[4]~[7]		: main cmd
	[8]~[11]	: sub cmd
	[12]		: response
	[13]~[16]	: data length
	[17]~[20]	: device
	[21]~[22]	: CRC
	*/

	// Header
	szComDataFrame[0] = (unsigned char)0xFF;
	szComDataFrame[1] = (unsigned char)0x55;
	szComDataFrame[2] = (unsigned char)0xEE;
	szComDataFrame[3] = (unsigned char)0xAA;
	// Data
	memmove(szComDataFrame + 4, &nMainCmd, 4);
	memmove(szComDataFrame + 8, &nSubCmd, 4);
	memmove(szComDataFrame + 12, &chResp, 1);
	memmove(szComDataFrame + 13, &nDataLength, 4);

	nCRC = MakeCRC16((char*)szComDataFrame, 17);
	memmove(szComDataFrame + 17, &nCRC, 4);


	//memmove(szComDataFrame + 17, nDataBuf, 4);

	// CRC-16
	nCRC = MakeCRC16((char*)szComDataFrame, READ_TX_SIZE - 2);
	memmove(szComDataFrame + (READ_TX_SIZE - 2), &nCRC, 2);


	//공유메모리에 write
	int nTxCnt = ComWrite(szComDataFrame, READ_TX_SIZE);

	BYTE szRxBuf[1024] = { 0 };

	if (READ_TX_SIZE == nTxCnt)
	{
		long timer = GetCurrentTime();

		while (nResult == STATE_DISCONNECT)
		{
			if (TickCheck(g_nTimeout, timer))
			{
				nResult = STATE_TIMEOUT;
				break;
			}

			if (ComGetc(&ch) <= 0)
				continue;

			switch (nRxCnt)
			{
			case 0: if ((unsigned char)ch == (unsigned char)0xFF) szRxBuf[nRxCnt++] = ch;	break;
			case 1: if ((unsigned char)ch == (unsigned char)0x55) szRxBuf[nRxCnt++] = ch;	break;
			case 2:	if ((unsigned char)ch == (unsigned char)0xEE) szRxBuf[nRxCnt++] = ch;	break;
			case 3:	if ((unsigned char)ch == (unsigned char)0xAA) szRxBuf[nRxCnt++] = ch;	break;
			default:
			{
				szRxBuf[nRxCnt++] = ch;

				if (nRxCnt >= READ_RX_SIZE)
				{
					nResult = STATE_CRC_ERR;

					memmove(&nCRC, &szRxBuf[nRxCnt - 2], 2);
					short int nCheckCRC = 0;
					nCheckCRC = MakeCRC16(szRxBuf, nRxCnt - 2);
					if (nCRC == nCheckCRC)
					{
						memmove(&nResult, &szRxBuf[17], 4);//data length : 16byte id, pos, di, do


														   //nAlarm = (pDeviceData->mDeviceID != nID) ? STATE_ID_ERR : STATE_SUCCESS;
					}
				}
			}break;
			}
		}//end while
	}

	return nResult;
}

DllExportExternC	int			ComEQConv1RunRpmWrite(int nData)
{

	int nMainCmd = 0;
	int nSubCmd = 20102;
	char szComDataFrame[128] = { 0 };
	int nDataLength = 8; //fixed
	short int nCRC = 0;
	unsigned char chResp = 0;
	char ch = 0;

	/* send message length = 27 byte
	[0]~[3]		: header
	[4]~[7]		: main cmd
	[8]~[11]	: sub cmd
	[12]		: response
	[13]~[16]	: data length
	[17]~[20]	: device
	[21]~[24]	: position
	[25]~[26]	: CRC
	*/

	// Header
	szComDataFrame[0] = (unsigned char)0xFF;
	szComDataFrame[1] = (unsigned char)0x55;
	szComDataFrame[2] = (unsigned char)0xEE;
	szComDataFrame[3] = (unsigned char)0xAA;
	// Data
	memmove(szComDataFrame + 4, &nMainCmd, 4);
	memmove(szComDataFrame + 8, &nSubCmd, 4);
	memmove(szComDataFrame + 12, &chResp, 1);
	memmove(szComDataFrame + 13, &nDataLength, 4);

	nCRC = MakeCRC16((char*)szComDataFrame, 17);

	memmove(szComDataFrame + 17, &nCRC, 4);
	memmove(szComDataFrame + 21, &nData, 4);

	// CRC-16
	nCRC = MakeCRC16((char*)szComDataFrame, (WRITE_TX_SIZE - 2));
	memmove(szComDataFrame + (WRITE_TX_SIZE - 2), &nCRC, 2);

	return (WRITE_TX_SIZE != ComWrite(szComDataFrame, WRITE_TX_SIZE)) ? STATE_DISCONNECT : STATE_SUCCESS;
}




//// Write
//DllExportExternC	int			MotionControlWrite(unsigned int nID, unsigned int nPos)
//{
//	int nMainCmd = 2;
//	int nSubCmd = 52;
//	char szComDataFrame[128] = { 0 };
//	int nDataLength = 8; //fixed
//	short int nCRC = 0;
//	unsigned char chResp = 0;
//	char ch = 0;
//
//	/* send message length = 27 byte
//	[0]~[3]		: header
//	[4]~[7]		: main cmd
//	[8]~[11]	: sub cmd
//	[12]		: response
//	[13]~[16]	: data length
//	[17]~[20]	: device
//	[21]~[24]	: position
//	[25]~[26]	: CRC
//	*/
//
//	// Header
//	szComDataFrame[0] = (unsigned char)0xFF;
//	szComDataFrame[1] = (unsigned char)0x55;
//	szComDataFrame[2] = (unsigned char)0xEE;
//	szComDataFrame[3] = (unsigned char)0xAA;
//	// Data
//	memmove(szComDataFrame + 4, &nMainCmd, 4);
//	memmove(szComDataFrame + 8, &nSubCmd, 4);
//	memmove(szComDataFrame + 12, &chResp, 1);
//	memmove(szComDataFrame + 13, &nDataLength, 4);
//	memmove(szComDataFrame + 17, &nID, 4);
//	memmove(szComDataFrame + 21, &nPos, 4);
//	// CRC-16
//	nCRC = MakeCRC16((char*)szComDataFrame, (WRITE_TX_SIZE - 2));
//	memmove(szComDataFrame + (WRITE_TX_SIZE - 2), &nCRC, 2);
//	
//
//
//
//
//	return (WRITE_TX_SIZE != ComWrite(szComDataFrame, WRITE_TX_SIZE)) ?  STATE_DISCONNECT : STATE_SUCCESS;
//}
//// Read
//DllExportExternC	int			MotionControlRead(unsigned int nID, LPDEVICE_DATA pDeviceData)
//{
//	//BOOL bResult = FALSE;
//
//	int nMainCmd = 2;
//	int nSubCmd = 54;
//	char szComDataFrame[128] = { 0 };
//	int nDataLength = 4; //fixed
//	short int nCRC = 0;
//	int nRxCnt = 0;
//	unsigned char chResp = 1;
//	char ch = 0;
//
//	int nAlarm = STATE_DISCONNECT;
//
//	//stShmData.mDeviceAlarm = STATE_DISCONNECT;
//	/* send message length = 23 byte
//	[0]~[3]		: header
//	[4]~[7]		: main cmd
//	[8]~[11]	: sub cmd
//	[12]		: response
//	[13]~[16]	: data length
//	[17]~[20]	: device
//	[21]~[22]	: CRC
//	*/
//
//	// Header
//	szComDataFrame[0] = (unsigned char)0xFF;
//	szComDataFrame[1] = (unsigned char)0x55;
//	szComDataFrame[2] = (unsigned char)0xEE;
//	szComDataFrame[3] = (unsigned char)0xAA;
//	// Data
//	memmove(szComDataFrame + 4, &nMainCmd, 4);
//	memmove(szComDataFrame + 8, &nSubCmd, 4);
//	memmove(szComDataFrame + 12, &chResp, 1);
//	memmove(szComDataFrame + 13, &nDataLength, 4);
//	memmove(szComDataFrame + 17, &nID, 4);
//
//	// CRC-16
//	nCRC = MakeCRC16((char*)szComDataFrame, READ_TX_SIZE - 2);
//	memmove(szComDataFrame + (READ_TX_SIZE - 2), &nCRC, 2);
//
//
//	//공유메모리에 write
//	int nTxCnt = ComWrite(szComDataFrame, READ_TX_SIZE);
//
//	BYTE szRxBuf[1024] = { 0 };
//
//	if (READ_TX_SIZE == nTxCnt)
//	{
//		long timer = GetCurrentTime();
//
//		while (nAlarm == STATE_DISCONNECT)
//		{
//			if (TickCheck(g_nTimeout, timer))
//			{
//				nAlarm = STATE_TIMEOUT;
//				break;
//			}
//
//			if (ComGetc(&ch) <= 0)
//				continue;
//
//			switch (nRxCnt)
//			{
//			case 0: if ((unsigned char)ch == (unsigned char)0xFF) szRxBuf[nRxCnt++] = ch;	break;
//			case 1: if ((unsigned char)ch == (unsigned char)0x55) szRxBuf[nRxCnt++] = ch;	break;
//			case 2:	if ((unsigned char)ch == (unsigned char)0xEE) szRxBuf[nRxCnt++] = ch;	break;
//			case 3:	if ((unsigned char)ch == (unsigned char)0xAA) szRxBuf[nRxCnt++] = ch;	break;
//			default:
//			{
//				szRxBuf[nRxCnt++] = ch;
//
//				if (nRxCnt >= READ_RX_SIZE)
//				{
//					nAlarm = STATE_CRC_ERR;
//
//					memmove(&nCRC, &szRxBuf[nRxCnt - 2], 2);
//					short int nCheckCRC = 0;
//					nCheckCRC = MakeCRC16(szRxBuf, nRxCnt - 2);
//					if (nCRC == nCheckCRC)
//					{
//						memmove(pDeviceData, &szRxBuf[17], 16);//data length : 16byte id, pos, di, do
//						nAlarm = (pDeviceData->mDeviceID != nID) ? STATE_ID_ERR : STATE_SUCCESS;
//					}					
//				}
//			}break;
//			}
//		}//end while
//	}
//
//	return nAlarm;
//}