#pragma once


/////////////////////////////////////////////////////////////////////////////////////////////
#define		Dllexport			__declspec(dllexport)
#define		Dllimport			__declspec(dllimport)
#define		DllExportExternC	extern "C" __declspec(dllexport)
#define		DllImportExternC	extern "C" __declspec(dllimport)
/////////////////////////////////////////////////////////////////////////////////////////////



#define	BUFF_SIZE				   4096

#define READ_RX_SIZE				   23
#define READ_TX_SIZE				   23
#define WRITE_RX_SIZE				   35
#define WRITE_TX_SIZE				   27

#define	STATE_SUCCESS				1
#define	STATE_DISCONNECT		   -1
#define	STATE_TIMEOUT			   -2
#define	STATE_CRC_ERR			   -4
#define	STATE_ID_ERR			   -8


//#pragma pack( push, 1 )
//typedef struct __deviceData
//{
//	unsigned int		mDeviceID;
//	unsigned int		mDevicePos;
//	short int			mDeviceDIHW;
//	short int			mDeviceDILW;
//	short int			mDeviceDOHW;
//	short int			mDeviceDOLW;
//
//}DEVICE_DATA, *LPDEVICE_DATA;
//#pragma pack(pop) 


DllExportExternC	BOOL			ComInitial(int nPort, int nTimeout);
DllExportExternC	BOOL			CommandDestroy();


DllExportExternC	int	     		ComEQModeRead();
//DllExportExternC	int	     		CommandEQEmergencyRead();
//DllExportExternC	int	     		CommandEQTactTimeRead();
//DllExportExternC	int	     		CommandEQUnitPerHourRead();

DllExportExternC	int	     		ComEQConv1RunRpmRead();
DllExportExternC	int	     		ComEQConv1RunRpmWrite(int nData);
//DllExportExternC	int	     		MotionControlWrite(unsigned int nID, unsigned int nPos);
//DllExportExternC	int 			MotionControlRead(unsigned int nID, LPDEVICE_DATA pDeviceData);