//************************************************************************************************************************************************
//
// File Name: Form1.cs
//
// Description:
//  The window and functionality of the pong game.
//
// Change History:
//  Author               Date           Description
//  Matthew D. Yorke     12/22/2017     Initial implementation that works in initial drawing of game objects and the ability to move both the
//                                      left and right paddles.
//
//************************************************************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   public partial class Form1 : Form
   {
      // The left paddle image depicitng the location and size of the image.
      Rectangle mLeftPaddle;

      // The right paddle image depicitng the location and size of the image.
      Rectangle mRightPaddle;

      // The ball image depicitng the location and size of the image.
      Rectangle mBall;

      // Determines if the up button for the left paddle is being pressed (true) or not (false).
      Boolean mLeftMoveUp;

      // Determines if the down button for the left paddle is being pressed (true) or not (false).
      Boolean mLeftMoveDown;

      // Determines if the up button for the right paddle is being pressed (true) or not (false).
      Boolean mRightMoveUp;

      // Determines if the down button for the left paddle is being pressed (true) or not (false).
      Boolean mRightMoveDown;

      // The time object that is used to periodically tick to update the screen.
      Timer timer;

      //*********************************************************************************************************************************************
      //
      // Method Name: Form1
      //
      // Description:
      //  Initialize the components used in the window. Set member variables to default values. Set the left and right paddle as well as the ball
      //  to their startgin locations. Determine which functions to call for painting and key presses. Start the periodic timer.
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      public Form1()
      {
         InitializeComponent();

         // Indicate the left paddle starts with no movement.
         mLeftMoveUp = false;
         mLeftMoveDown = false;

         // Indicate the right paddle starts with no movement.
         mRightMoveUp = false;
         mRightMoveDown = false;

         // Start the left paddle at the upper left corner of the window (with padding from the goal).
         mLeftPaddle = new Rectangle(PongConstants.PADDLE_BOUNDARY_PADDING,
                                     PongConstants.PADDLE_STARTING_Y_COORDINATE,
                                     PongConstants.PADDLE_WIDTH,
                                     PongConstants.PADDLE_HEIGHT);
         
         // Start the right paddle at the upper right corner of the window (with padding from the goal).
         mRightPaddle = new Rectangle(this.Size.Width - PongConstants.PADDLE_WIDTH - PongConstants.PADDLE_BOUNDARY_PADDING,
                                      PongConstants.PADDLE_STARTING_Y_COORDINATE,
                                      PongConstants.PADDLE_WIDTH,
                                      PongConstants.PADDLE_HEIGHT);

         // Start the ball at the center of the window.
         mBall = new Rectangle((this.Size.Width / PongConstants.HALF) - (PongConstants.BALL_WIDTH_AND_HEIGHT / PongConstants.HALF),
                               (this.Size.Height / PongConstants.HALF) - (PongConstants.BALL_WIDTH_AND_HEIGHT / PongConstants.HALF),
                               PongConstants.BALL_WIDTH_AND_HEIGHT,
                               PongConstants.BALL_WIDTH_AND_HEIGHT);
   
         // Indicate the painting callback method to use when a repaint is called.
         this.Paint += Draw;

         // Indicate that key events will be used in this window.
         this.KeyPreview = true;
         // Indicate the callback method to use when a key is pressed down.
         this.KeyDown += Form1_KeyDown;
         // Indicate the callback method to use when a key is released.
         this.KeyUp += Form1_KeyUp;
         
         // Start up the periodic timer.
         timer = new Timer();
         timer.Tick += new EventHandler(TimerTick);
         timer.Interval = PongConstants.TIMER_INTERVAL;
         timer.Enabled = true;
         timer.Start();
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: Form1_Load
      //
      // Description:
      //  TODO: Add description. As it stands now this is method is needed to run the program, but has no implementation.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void Form1_Load(object theSender, EventArgs theEventArguments)
      {
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: Form1_KeyDown
      //
      // Description:
      //  Determine which key was pressed down:
      //      W - Let the program to know to move the left paddle up on the next update.
      //      S - Let the program to know to move the left paddle down on the next update.
      //      O - Let the program to know to move the right paddle up on the next update.
      //      L - Let the program to know to move the right paddle down on the next update.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void Form1_KeyDown(Object theSender, KeyEventArgs theEventArguments)
      {
         switch (theEventArguments.KeyCode)
         {
            // The W key is used for the left paddle up movement. Indicate the left paddle is now being moved upwards.
            case Keys.W:
            {
               mLeftMoveUp = true;
               break;
            }
            // The W key is used for the left paddle down movement. Indicate the left paddle is now being moved downwards.
            case Keys.S:
            {
               mLeftMoveDown = true;
               break;
            }
            // The W key is used for the right paddle up movement. Indicate the right paddle is now being moved upwards.
            case Keys.O:
            {
               mRightMoveUp = true;
               break;
            }
            // The W key is used for the right paddle down movement. Indicate the right paddle is now being moved downwards.
            case Keys.L:
            {
               mRightMoveDown = true;
               break;
            }
            default:
            {
               break;
            }
         }
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: Form1_KeyUp
      //
      // Description:
      //  Determine which key was release:
      //      W - Let the program to know to no longer move the left paddle up on the next update.
      //      S - Let the program to know to no longer move the left paddle down on the next update.
      //      O - Let the program to know to no longer move the right paddle up on the next update.
      //      L - Let the program to know to no longer move the right paddle down on the next update.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void Form1_KeyUp(Object theSender, KeyEventArgs theEventArguments)
      {
         // Determine which key is being released.
         switch (theEventArguments.KeyCode)
         {
            // The W key is used for the left paddle up movement. Indicate the left paddle is no longer being moved upwards.
            case Keys.W:
            {
               mLeftMoveUp = false;
               break;
            }
            // The S key is used for the left paddle Down movement. Indicate the left paddle is no longer being moved downwards.
            case Keys.S:
            {
               mLeftMoveDown = false;
               break;
            }
            // The O key is used for the right paddle up movement. Indicate the right paddle is no longer being moved upwards.
            case Keys.O:
            {
               mRightMoveUp = false;
               break;
            }
            // The L key is used for the right paddle down movement. Indicate the right paddle is no longer being moved downwards.
            case Keys.L:
            {
               mRightMoveDown = false;
               break;
            }
            default:
            {
               break;
            }
         }
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: TimerTick
      //
      // Description:
      //  Call to update the object in the window and then indicate a redraw is needed.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void TimerTick(Object theSender, EventArgs theEventArguments)
      {
         // Update the objects on the window.
         UpdateWindow();
         // Call to redraw the object on the window.
         Invalidate();
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: UpdateWindow
      //
      // Description:
      //  Call to update the objects in the game.
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void UpdateWindow()
      {
         // Update the paddles on the window based on the keys pressed down or released.
         UpdatePaddle(mLeftMoveUp, mLeftMoveDown, ref mLeftPaddle);
         UpdatePaddle(mRightMoveUp, mRightMoveDown, ref mRightPaddle);
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: UpdatePaddle
      //
      // Description:
      //  Call to update the paddle positions in the game.
      //
      // Arguments:
      //  theUp - Determine if the up key for the paddle is pressed (true) or not (false).
      //  theDown - Determine if the up key for the paddle is pressed (true) or not (false).
      //  thePaddle - Reference of which paddle is being updated
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void UpdatePaddle(Boolean theUp, Boolean theDown, ref Rectangle thePaddle)
      {
         // Check if the up button for the paddle is currently being pressed down.
         // Note: If both up AND down buttons are pressed the up button takes presedence.
         if (theUp == true)
         {
            // Set the paddle y-coordinate to draw further up the window by the paddle speed.
            thePaddle.Y -= PongConstants.PADDLE_SPEED;

            // Prevent the paddle from going out of bounds at the top of the window.
            if(thePaddle.Y < PongConstants.PADDLE_STARTING_Y_COORDINATE)
            {
               thePaddle.Y = PongConstants.PADDLE_STARTING_Y_COORDINATE;
            }
         }
         // Check if the down button for the paddle is currently being pressed down.
         else if (theDown == true)
         {
            // Set the paddle y-coordinate to draw further down the window by the paddle speed.
            thePaddle.Y += PongConstants.PADDLE_SPEED;

            // Prevent the paddle from going out of bounds at the bottom of the window.
            if(thePaddle.Y > (this.Size.Height - PongConstants.PADDLE_HEIGHT))
            {
               thePaddle.Y = this.Size.Height - PongConstants.PADDLE_HEIGHT;
            }
         }
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: Draw
      //
      // Description:
      //  Draw the game objects ontop the window.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void Draw(Object theSender, PaintEventArgs theEventArguments)
      {
         // Note: This is temporary brush being used for preliminary coding.
         SolidBrush sb = new SolidBrush(Color.Green);

         // Draw the paddle and balls onto the window.
         theEventArguments.Graphics.FillRectangle(sb, mLeftPaddle);
         theEventArguments.Graphics.FillRectangle(sb, mRightPaddle);
         theEventArguments.Graphics.FillRectangle(sb, mBall);
      }
   }
}
