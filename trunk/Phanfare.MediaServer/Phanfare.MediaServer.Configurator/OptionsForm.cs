using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Phanfare.MediaServer.Utilities;

namespace Phanfare.MediaServer.Configurator
{
	partial class OptionsForm : Form
	{
		public OptionsForm()
		{
			InitializeComponent();

			IniReader reader = new IniReader( this.SettingsFilePath );
			this.emailTextBox.Text = reader.ReadString( "Phanfare", "EmailAddress", string.Empty );
			string password = reader.ReadString( "Phanfare", "Password", string.Empty );
			if( string.IsNullOrEmpty( password ) == true )
				this.passwordTextBox.Text = string.Empty;
			else
				this.passwordTextBox.Text = Security.Decrypt( password );
			this.showHiddenCheckBox.Checked = reader.ReadBoolean( "Security", "ShowHidden", false );

			this.Activate();
			this.BringToFront();
			this.TopMost = true;

			Timer t = new Timer();
			t.Interval = 1000;
			t.Start();
			t.Tick += new EventHandler( t_Tick );
		}

		private void t_Tick( object sender, EventArgs e )
		{
			this.BringToFront();
			this.Activate();
			Timer t = ( Timer )sender;
			t.Dispose();
		}

		public const string SettingsFileName = "Phanfare.MediaServer.ini";
		public string SettingsFilePath
		{
			get
			{
				string executionPath = Path.GetDirectoryName( typeof( OptionsForm ).Assembly.Location );
				return Path.Combine( executionPath, SettingsFileName );
			}
		}

		private void saveButton_Click( object sender, EventArgs e )
		{
			IniReader writer = new IniReader( this.SettingsFilePath );
			writer.Write( "Phanfare", "EmailAddress", this.emailTextBox.Text );
			writer.Write( "Phanfare", "Password", Security.Encrypt( this.passwordTextBox.Text ) );
			writer.Write( "Security", "ShowHidden", this.showHiddenCheckBox.Checked );
			this.Close();
		}

		private void cancelButton_Click( object sender, EventArgs e )
		{
			this.Close();
		}
	}
}
