using Intel.UPNP;

namespace Phanfare.MediaServer
{
    /// <summary>
    /// Transparent DeviceSide UPnP Service
    /// </summary>
    public class DvMediaReceiverRegistrar : IUPnPService
    {

        // Place your declarations above this line

        #region AutoGenerated Code Section [Do NOT Modify, unless you know what you're doing]
        //{{{{{ Begin Code Block

        private _DvMediaReceiverRegistrar _S;
        public static string URN = "urn:microsoft.com:service:X_MS_MediaReceiverRegistrar:1";
        public double VERSION
        {
           get
           {
               return(double.Parse(_S.GetUPnPService().Version));
           }
        }

        public delegate void OnStateVariableModifiedHandler(DvMediaReceiverRegistrar sender);
        public event OnStateVariableModifiedHandler OnStateVariableModified_AuthorizationDeniedUpdateID;
        public event OnStateVariableModifiedHandler OnStateVariableModified_A_ARG_TYPE_DeviceID;
        public event OnStateVariableModifiedHandler OnStateVariableModified_A_ARG_TYPE_RegistrationRespMsg;
        public event OnStateVariableModifiedHandler OnStateVariableModified_ValidationRevokedUpdateID;
        public event OnStateVariableModifiedHandler OnStateVariableModified_ValidationSucceededUpdateID;
        public event OnStateVariableModifiedHandler OnStateVariableModified_A_ARG_TYPE_Result;
        public event OnStateVariableModifiedHandler OnStateVariableModified_AuthorizationGrantedUpdateID;
        public event OnStateVariableModifiedHandler OnStateVariableModified_A_ARG_TYPE_RegistrationReqMsg;
        public System.UInt32 Evented_AuthorizationDeniedUpdateID
        {
            get
            {
               return((System.UInt32)_S.GetStateVariable("AuthorizationDeniedUpdateID"));
            }
            set
            {
               _S.SetStateVariable("AuthorizationDeniedUpdateID", value);
            }
        }
        public System.String A_ARG_TYPE_DeviceID
        {
            get
            {
               return((System.String)_S.GetStateVariable("A_ARG_TYPE_DeviceID"));
            }
            set
            {
               _S.SetStateVariable("A_ARG_TYPE_DeviceID", value);
            }
        }
        public System.Byte[] A_ARG_TYPE_RegistrationRespMsg
        {
            get
            {
               return((System.Byte[])_S.GetStateVariable("A_ARG_TYPE_RegistrationRespMsg"));
            }
            set
            {
               _S.SetStateVariable("A_ARG_TYPE_RegistrationRespMsg", value);
            }
        }
        public System.UInt32 Evented_ValidationRevokedUpdateID
        {
            get
            {
               return((System.UInt32)_S.GetStateVariable("ValidationRevokedUpdateID"));
            }
            set
            {
               _S.SetStateVariable("ValidationRevokedUpdateID", value);
            }
        }
        public System.UInt32 Evented_ValidationSucceededUpdateID
        {
            get
            {
               return((System.UInt32)_S.GetStateVariable("ValidationSucceededUpdateID"));
            }
            set
            {
               _S.SetStateVariable("ValidationSucceededUpdateID", value);
            }
        }
        public System.Int32 A_ARG_TYPE_Result
        {
            get
            {
               return((System.Int32)_S.GetStateVariable("A_ARG_TYPE_Result"));
            }
            set
            {
               _S.SetStateVariable("A_ARG_TYPE_Result", value);
            }
        }
        public System.UInt32 Evented_AuthorizationGrantedUpdateID
        {
            get
            {
               return((System.UInt32)_S.GetStateVariable("AuthorizationGrantedUpdateID"));
            }
            set
            {
               _S.SetStateVariable("AuthorizationGrantedUpdateID", value);
            }
        }
        public System.Byte[] A_ARG_TYPE_RegistrationReqMsg
        {
            get
            {
               return((System.Byte[])_S.GetStateVariable("A_ARG_TYPE_RegistrationReqMsg"));
            }
            set
            {
               _S.SetStateVariable("A_ARG_TYPE_RegistrationReqMsg", value);
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_AuthorizationDeniedUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationDeniedUpdateID")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationDeniedUpdateID")).Accumulator = value;
            }
        }
        public double ModerationDuration_AuthorizationDeniedUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationDeniedUpdateID")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationDeniedUpdateID")).ModerationPeriod = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_A_ARG_TYPE_DeviceID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_DeviceID")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_DeviceID")).Accumulator = value;
            }
        }
        public double ModerationDuration_A_ARG_TYPE_DeviceID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_DeviceID")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_DeviceID")).ModerationPeriod = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_A_ARG_TYPE_RegistrationRespMsg
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationRespMsg")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationRespMsg")).Accumulator = value;
            }
        }
        public double ModerationDuration_A_ARG_TYPE_RegistrationRespMsg
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationRespMsg")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationRespMsg")).ModerationPeriod = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_ValidationRevokedUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationRevokedUpdateID")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationRevokedUpdateID")).Accumulator = value;
            }
        }
        public double ModerationDuration_ValidationRevokedUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationRevokedUpdateID")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationRevokedUpdateID")).ModerationPeriod = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_ValidationSucceededUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationSucceededUpdateID")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationSucceededUpdateID")).Accumulator = value;
            }
        }
        public double ModerationDuration_ValidationSucceededUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationSucceededUpdateID")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("ValidationSucceededUpdateID")).ModerationPeriod = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_A_ARG_TYPE_Result
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_Result")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_Result")).Accumulator = value;
            }
        }
        public double ModerationDuration_A_ARG_TYPE_Result
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_Result")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_Result")).ModerationPeriod = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_AuthorizationGrantedUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationGrantedUpdateID")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationGrantedUpdateID")).Accumulator = value;
            }
        }
        public double ModerationDuration_AuthorizationGrantedUpdateID
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationGrantedUpdateID")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("AuthorizationGrantedUpdateID")).ModerationPeriod = value;
            }
        }
        public UPnPModeratedStateVariable.IAccumulator Accumulator_A_ARG_TYPE_RegistrationReqMsg
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationReqMsg")).Accumulator);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationReqMsg")).Accumulator = value;
            }
        }
        public double ModerationDuration_A_ARG_TYPE_RegistrationReqMsg
        {
            get
            {
                 return(((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationReqMsg")).ModerationPeriod);
            }
            set
            {
                 ((UPnPModeratedStateVariable)_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationReqMsg")).ModerationPeriod = value;
            }
        }
        public delegate void Delegate_IsAuthorized(System.String DeviceID, out System.Int32 Result);
        public delegate void Delegate_IsValidated(System.String DeviceID, out System.Int32 Result);
        public delegate void Delegate_RegisterDevice(System.Byte[] RegistrationReqMsg, out System.Byte[] RegistrationRespMsg);

        public Delegate_IsAuthorized External_IsAuthorized = null;
        public Delegate_IsValidated External_IsValidated = null;
        public Delegate_RegisterDevice External_RegisterDevice = null;

        public void RemoveStateVariable_AuthorizationDeniedUpdateID()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("AuthorizationDeniedUpdateID"));
        }
        public void RemoveStateVariable_A_ARG_TYPE_DeviceID()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_DeviceID"));
        }
        public void RemoveStateVariable_A_ARG_TYPE_RegistrationRespMsg()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationRespMsg"));
        }
        public void RemoveStateVariable_ValidationRevokedUpdateID()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("ValidationRevokedUpdateID"));
        }
        public void RemoveStateVariable_ValidationSucceededUpdateID()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("ValidationSucceededUpdateID"));
        }
        public void RemoveStateVariable_A_ARG_TYPE_Result()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_Result"));
        }
        public void RemoveStateVariable_AuthorizationGrantedUpdateID()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("AuthorizationGrantedUpdateID"));
        }
        public void RemoveStateVariable_A_ARG_TYPE_RegistrationReqMsg()
        {
            _S.GetUPnPService().RemoveStateVariable(_S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationReqMsg"));
        }
        public void RemoveAction_IsAuthorized()
        {
             _S.GetUPnPService().RemoveMethod("IsAuthorized");
        }
        public void RemoveAction_IsValidated()
        {
             _S.GetUPnPService().RemoveMethod("IsValidated");
        }
        public void RemoveAction_RegisterDevice()
        {
             _S.GetUPnPService().RemoveMethod("RegisterDevice");
        }
        public System.Net.IPEndPoint GetCaller()
        {
             return(_S.GetUPnPService().GetCaller());
        }
        public System.Net.IPEndPoint GetReceiver()
        {
             return(_S.GetUPnPService().GetReceiver());
        }

        private class _DvMediaReceiverRegistrar
        {
            private DvMediaReceiverRegistrar Outer = null;
            private UPnPService S;
            internal _DvMediaReceiverRegistrar(DvMediaReceiverRegistrar n)
            {
                Outer = n;
                S = BuildUPnPService();
            }
            public UPnPService GetUPnPService()
            {
                return(S);
            }
            public void SetStateVariable(string VarName, object VarValue)
            {
               S.SetStateVariable(VarName,VarValue);
            }
            public object GetStateVariable(string VarName)
            {
               return(S.GetStateVariable(VarName));
            }
            protected UPnPService BuildUPnPService()
            {
                UPnPStateVariable[] RetVal = new UPnPStateVariable[8];
                RetVal[0] = new UPnPModeratedStateVariable("AuthorizationDeniedUpdateID", typeof(System.UInt32), true);
                RetVal[1] = new UPnPModeratedStateVariable("A_ARG_TYPE_DeviceID", typeof(System.String), false);
                RetVal[1].AddAssociation("IsAuthorized", "DeviceID");
                RetVal[1].AddAssociation("IsValidated", "DeviceID");
                RetVal[2] = new UPnPModeratedStateVariable("A_ARG_TYPE_RegistrationRespMsg", typeof(System.Byte[]), false);
                RetVal[2].AddAssociation("RegisterDevice", "RegistrationRespMsg");
                RetVal[3] = new UPnPModeratedStateVariable("ValidationRevokedUpdateID", typeof(System.UInt32), true);
                RetVal[4] = new UPnPModeratedStateVariable("ValidationSucceededUpdateID", typeof(System.UInt32), true);
                RetVal[5] = new UPnPModeratedStateVariable("A_ARG_TYPE_Result", typeof(System.Int32), false);
                RetVal[5].AddAssociation("IsAuthorized", "Result");
                RetVal[5].AddAssociation("IsValidated", "Result");
                RetVal[6] = new UPnPModeratedStateVariable("AuthorizationGrantedUpdateID", typeof(System.UInt32), true);
                RetVal[7] = new UPnPModeratedStateVariable("A_ARG_TYPE_RegistrationReqMsg", typeof(System.Byte[]), false);
                RetVal[7].AddAssociation("RegisterDevice", "RegistrationReqMsg");

                UPnPService S = new UPnPService(1, "urn:microsoft.com:serviceId:X_MS_MediaReceiverRegistrar", "urn:microsoft.com:service:X_MS_MediaReceiverRegistrar:1", true, this);
                for(int i=0;i<RetVal.Length;++i)
                {
                   S.AddStateVariable(RetVal[i]);
                }
                S.AddMethod("IsAuthorized");
                S.AddMethod("IsValidated");
                S.AddMethod("RegisterDevice");
                return(S);
            }

            public void IsAuthorized(System.String DeviceID, out System.Int32 Result)
            {
                if(Outer.External_IsAuthorized != null)
                {
                    Outer.External_IsAuthorized(DeviceID, out Result);
                }
                else
                {
                    Sink_IsAuthorized(DeviceID, out Result);
                }
            }
            public void IsValidated(System.String DeviceID, out System.Int32 Result)
            {
                if(Outer.External_IsValidated != null)
                {
                    Outer.External_IsValidated(DeviceID, out Result);
                }
                else
                {
                    Sink_IsValidated(DeviceID, out Result);
                }
            }
            public void RegisterDevice(System.Byte[] RegistrationReqMsg, out System.Byte[] RegistrationRespMsg)
            {
                if(Outer.External_RegisterDevice != null)
                {
                    Outer.External_RegisterDevice(RegistrationReqMsg, out RegistrationRespMsg);
                }
                else
                {
                    Sink_RegisterDevice(RegistrationReqMsg, out RegistrationRespMsg);
                }
            }

            public Delegate_IsAuthorized Sink_IsAuthorized;
            public Delegate_IsValidated Sink_IsValidated;
            public Delegate_RegisterDevice Sink_RegisterDevice;
        }
        public DvMediaReceiverRegistrar()
        {
            _S = new _DvMediaReceiverRegistrar(this);
            _S.GetUPnPService().GetStateVariableObject("AuthorizationDeniedUpdateID").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_AuthorizationDeniedUpdateID);
            _S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_DeviceID").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_A_ARG_TYPE_DeviceID);
            _S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationRespMsg").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_A_ARG_TYPE_RegistrationRespMsg);
            _S.GetUPnPService().GetStateVariableObject("ValidationRevokedUpdateID").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_ValidationRevokedUpdateID);
            _S.GetUPnPService().GetStateVariableObject("ValidationSucceededUpdateID").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_ValidationSucceededUpdateID);
            _S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_Result").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_A_ARG_TYPE_Result);
            _S.GetUPnPService().GetStateVariableObject("AuthorizationGrantedUpdateID").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_AuthorizationGrantedUpdateID);
            _S.GetUPnPService().GetStateVariableObject("A_ARG_TYPE_RegistrationReqMsg").OnModified += new UPnPStateVariable.ModifiedHandler(OnModifiedSink_A_ARG_TYPE_RegistrationReqMsg);
            _S.Sink_IsAuthorized = new Delegate_IsAuthorized(IsAuthorized);
            _S.Sink_IsValidated = new Delegate_IsValidated(IsValidated);
            _S.Sink_RegisterDevice = new Delegate_RegisterDevice(RegisterDevice);
        }
        public DvMediaReceiverRegistrar(string ID):this()
        {
            _S.GetUPnPService().ServiceID = ID;
        }
        public UPnPService GetUPnPService()
        {
            return(_S.GetUPnPService());
        }
        private void OnModifiedSink_AuthorizationDeniedUpdateID(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_AuthorizationDeniedUpdateID != null) OnStateVariableModified_AuthorizationDeniedUpdateID(this);
        }
        private void OnModifiedSink_A_ARG_TYPE_DeviceID(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_A_ARG_TYPE_DeviceID != null) OnStateVariableModified_A_ARG_TYPE_DeviceID(this);
        }
        private void OnModifiedSink_A_ARG_TYPE_RegistrationRespMsg(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_A_ARG_TYPE_RegistrationRespMsg != null) OnStateVariableModified_A_ARG_TYPE_RegistrationRespMsg(this);
        }
        private void OnModifiedSink_ValidationRevokedUpdateID(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_ValidationRevokedUpdateID != null) OnStateVariableModified_ValidationRevokedUpdateID(this);
        }
        private void OnModifiedSink_ValidationSucceededUpdateID(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_ValidationSucceededUpdateID != null) OnStateVariableModified_ValidationSucceededUpdateID(this);
        }
        private void OnModifiedSink_A_ARG_TYPE_Result(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_A_ARG_TYPE_Result != null) OnStateVariableModified_A_ARG_TYPE_Result(this);
        }
        private void OnModifiedSink_AuthorizationGrantedUpdateID(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_AuthorizationGrantedUpdateID != null) OnStateVariableModified_AuthorizationGrantedUpdateID(this);
        }
        private void OnModifiedSink_A_ARG_TYPE_RegistrationReqMsg(UPnPStateVariable sender, object NewValue)
        {
            if(OnStateVariableModified_A_ARG_TYPE_RegistrationReqMsg != null) OnStateVariableModified_A_ARG_TYPE_RegistrationReqMsg(this);
        }
        //}}}}} End of Code Block

        #endregion

        /// <summary>
        /// Action: IsAuthorized
        /// </summary>
        /// <param name="DeviceID">Associated State Variable: A_ARG_TYPE_DeviceID</param>
        /// <param name="Result">Associated State Variable: A_ARG_TYPE_Result</param>
        public void IsAuthorized(System.String DeviceID, out System.Int32 Result)
        {
            //ToDo: Add Your implementation here, and remove exception
            throw(new UPnPCustomException(800,"This method has not been completely implemented..."));
        }
        /// <summary>
        /// Action: IsValidated
        /// </summary>
        /// <param name="DeviceID">Associated State Variable: A_ARG_TYPE_DeviceID</param>
        /// <param name="Result">Associated State Variable: A_ARG_TYPE_Result</param>
        public void IsValidated(System.String DeviceID, out System.Int32 Result)
        {
            //ToDo: Add Your implementation here, and remove exception
            throw(new UPnPCustomException(800,"This method has not been completely implemented..."));
        }
        /// <summary>
        /// Action: RegisterDevice
        /// </summary>
        /// <param name="RegistrationReqMsg">Associated State Variable: A_ARG_TYPE_RegistrationReqMsg</param>
        /// <param name="RegistrationRespMsg">Associated State Variable: A_ARG_TYPE_RegistrationRespMsg</param>
        public void RegisterDevice(System.Byte[] RegistrationReqMsg, out System.Byte[] RegistrationRespMsg)
        {
            //ToDo: Add Your implementation here, and remove exception
            throw(new UPnPCustomException(800,"This method has not been completely implemented..."));
        }
    }
}