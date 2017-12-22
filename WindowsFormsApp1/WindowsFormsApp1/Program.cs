//************************************************************************************************************************************************
//
// File Name: Program.cs
//
// Description:
//  The entrance for the when the program starts. the Main fucntion is located here and indicates to create the window used for pong.
//
// Change History:
//  Author               Date           Description
//  Matthew D. Yorke     12/22/2017     Initial release of this class that kicks off the program.
//
//************************************************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   static class Program
   {
      //*********************************************************************************************************************************************
      //
      // Method Name: Main
      //
      // Description:
      //  The main entrance into the program that kicks off the window for the pong game.
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new Form1());
      }
   }
}
