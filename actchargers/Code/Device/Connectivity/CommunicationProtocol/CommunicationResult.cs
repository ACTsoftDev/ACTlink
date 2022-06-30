namespace actchargers
{
    public enum CommunicationResult : uint     //represents the type of communication result when doing the COMM. with the UART module
    {
        FTDI_OPENING_ERROR = 0,                   //Error: error in opening the ftdi device
        SENDING_ERROR = 1,                        //Error: error in sending the series of bytes representing the packet to the microcontroller
        EXPECTED_DATA_COUNT_ERROR = 2,            //Error: there must be an expected min and max count of data bytes to be received from the microcontroller, so if it was out the ranges it will return this error 
        RECEIVING_ERROR = 3,                      //Error: error in receiving the series of bytes representing the packet to be received from the microcontroller
        PACKET_SIZE_ERROR = 4,                    //Error: generated when the packet size does not equate to the first byte of the packet plus one
        CRC_ERROR = 5,                            //Error: when computing the crc for the received packet and the result is not zero, then this error is generated 
        RECEIVER_KEY_ERROR = 6,                   //Error: generated when there is an error in the ID for the reponding microcontroller
        ACK_NACK_ERROR = 7,                       //Error: generated when the ack nack byte does not meet the ack or nack property
        OK = 8,                                    // this result represents that all communication process was well
        ERROR_IN_EEPROM_READING = 9,
        ERROR_IN_EEPROM_WRITING = 10,
        ACCESS_ERROR = 11,
        SIZEERROR = 12,
        ReadSomethingElse = 13,
        CHARGER_BUSY = 14,
        COMMAND_DELAYED = 15,
        NOT_EXIST = 16,
        invalidArg = 17,
        internalFailure = 18,
        mutexKilled = 19,
        holdMobileMode = 20,
    }
}
