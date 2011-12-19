namespace VixenModules.Output.DmxUsbPro
{
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;

    using CommonElements;

    using Vixen.Commands;
    using Vixen.Module.Output;

    public class Module : OutputModuleInstanceBase
    {
        private SerialPort _serialPort;

        private DmxUsbProSender _dmxUsbProSender;

        public override bool HasSetup
        {
            get
            {
                return true;
            }
        }

        public override bool Setup()
        {
            using (var portConfig = new SerialPortConfig(this._serialPort))
            {
                if (portConfig.ShowDialog() == DialogResult.OK)
                {
                    this._serialPort = portConfig.SelectedPort;
					if(_serialPort != null) {
						this._serialPort.Handshake = Handshake.None;
						this._serialPort.Encoding = Encoding.UTF8;

						// Write back to setup
						var data = GetModuleData();
						data.PortName = _serialPort.PortName;
						data.BaudRate = _serialPort.BaudRate;
						data.Partity = _serialPort.Parity;
						data.DataBits = _serialPort.DataBits;
						data.StopBits = _serialPort.StopBits;
						return true;
					}
                }
                
                return false;
            }
        }

        public override void Start()
        {
            if (this._dmxUsbProSender != null)
            {
                this._dmxUsbProSender.Dispose();
            }

            this.InitializePort();
            this._dmxUsbProSender = new DmxUsbProSender(this._serialPort);
            this._dmxUsbProSender.Start();
            base.Start();
        }

        public override void Dispose()
        {
            if (this._serialPort != null)
            {
                if (this._serialPort.IsOpen)
                {
                    this._serialPort.Close();
                    this._serialPort.Dispose();
                    this._serialPort = null;
                }
            }

            base.Dispose();
        }

        public override void Stop()
        {
            this._dmxUsbProSender.Stop();
            if (this._serialPort != null)
            {
                if (this._serialPort.IsOpen)
                {
                    this._serialPort.Close();
                    this._serialPort.Dispose();
                    this._serialPort = null;
                }
            }

            base.Stop();
        }

        protected override void _SetOutputCount(int outputCount)
        {            
        }
       
        protected override void _UpdateState(Command[] outputStates)
        {
            this._dmxUsbProSender.SendDmxPacket(outputStates);
        }

        private Data GetModuleData()
        {
            return (Data)this.ModuleData;
        }

        private void InitializePort()
        {
            // Recreate serial port based on setup data
            if (this._serialPort != null && this._serialPort.IsOpen)
            {
                this._serialPort.Close();
                this._serialPort.Dispose();
            }

            var data = this.GetModuleData();
			if(data.PortName != null) {
				this._serialPort = new SerialPort(data.PortName, data.BaudRate, data.Partity, data.DataBits, data.StopBits)
				                   {
				                   	Handshake = Handshake.None, Encoding = Encoding.UTF8
				                   };
			}
        }
    }
}