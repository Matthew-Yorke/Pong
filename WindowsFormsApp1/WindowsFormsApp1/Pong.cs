//************************************************************************************************************************************************
//
// File Name: Pong.cs
//
// Description:
//  The window and functionality of the Pong game. This window handles the updating and drawing of all game components of the game including
//  the paddles, ball, text. This class also handles the collision detection for the game.
//
// Change History:
//  Author               Date           Description
//  Matthew D. Yorke     12/22/2017     Initial implementation that works in initial drawing of game objects and the ability to move both the
//                                      left and right paddles.
//  Matthew D. Yorke     03/21/2018     Update the game to handle the movement of the ball, collision detection and score keeping as well as
//                                      handling matches of the game including prematch setup.
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
   public partial class Pong : Form
   {
      // The current score of the left player.
      int mLeftPlayerScore;

      // The current score of the right player.
      int mRightPlayerScore;

      // Determines if the game is in a period of wait before a match starts.
      // Note: While in prematch wait the scores are displayed and a countdown timer is displayed until the next match starts.
      bool mPrematchWait;

      // The amount of time that is left before the next match starts.
      int mPrematchTime;

      // Determines if the up button for the left paddle is being pressed (true) or not (false).
      Boolean mLeftMoveUp;

      // Determines if the down button for the left paddle is being pressed (true) or not (false).
      Boolean mLeftMoveDown;

      // Determines if the up button for the right paddle is being pressed (true) or not (false).
      Boolean mRightMoveUp;

      // Determines if the down button for the left paddle is being pressed (true) or not (false).
      Boolean mRightMoveDown;

      // The left paddle image depicting the location and size of the image.
      Rectangle mLeftPaddle;

      // The right paddle image depicting the location and size of the image.
      Rectangle mRightPaddle;

      // The ball image depicting the location and size of the image.
      Rectangle mBall;

      // The possible vertical directions the ball can go in a 2D space.
      enum VerticalDirection {UP, DOWN};

      // The possible horizontal directions the ball can go in a 2D space.
      enum HorizontalDirection {LEFT, RIGHT};

      // Determines the balls current vertical direction.
      VerticalDirection mBallVerticalDirection;

      // Determines the balls current horizontal direction.
      HorizontalDirection mBallHorizontalDirection;

      // The speed in which the ball moves.
      int mBallSpeed;

      // The time object that is used to periodically tick to update the screen.
      Timer mTimer;

      //*********************************************************************************************************************************************
      //
      // Method Name: Pong
      //
      // Description:
      //  Initialize the components used in the window. Set member variables to default values. Set the left and right paddle as well as the ball
      //  to their starting locations. Determine which functions to call for painting and key presses. Start the periodic timer.
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      public Pong()
      {
         // Call to initialize the screen and components on the screen.
         InitializeComponent();

         // Player scores start at 0 in a new game.
         mLeftPlayerScore = PongConstants.PLAYER_INITIAL_SCORE;
         mRightPlayerScore = PongConstants.PLAYER_INITIAL_SCORE;

         // The game starts in prematch wait to give players time to prepare for the coming match.
         mPrematchWait = true;
         mPrematchTime = PongConstants.PREMATCH_WAIT_TIME;

         // Indicate the left paddle starts with no movement.
         mLeftMoveUp = false;
         mLeftMoveDown = false;

         // Indicate the right paddle starts with no movement.
         mRightMoveUp = false;
         mRightMoveDown = false;

         // Start the left paddle at the upper left corner of the window (with padding from the goal).
         mLeftPaddle = new Rectangle(PongConstants.PADDLE_BOUNDARY_PADDING,
                                     (this.Size.Height / PongConstants.HALF) - (PongConstants.PADDLE_HEIGHT / PongConstants.HALF),
                                     PongConstants.PADDLE_WIDTH,
                                     PongConstants.PADDLE_HEIGHT);
         
         // Start the right paddle at the upper right corner of the window (with padding from the goal).
         mRightPaddle = new Rectangle(this.Size.Width - PongConstants.PADDLE_WIDTH - PongConstants.PADDLE_BOUNDARY_PADDING,
                                      (this.Size.Height / PongConstants.HALF) - (PongConstants.PADDLE_HEIGHT / PongConstants.HALF),
                                      PongConstants.PADDLE_WIDTH,
                                      PongConstants.PADDLE_HEIGHT);

         // Start the ball at the center of the window.
         mBall = new Rectangle((this.Size.Width / PongConstants.HALF) - (PongConstants.BALL_WIDTH_AND_HEIGHT / PongConstants.HALF),
                               (this.Size.Height / PongConstants.HALF) - (PongConstants.BALL_WIDTH_AND_HEIGHT / PongConstants.HALF),
                               PongConstants.BALL_WIDTH_AND_HEIGHT,
                               PongConstants.BALL_WIDTH_AND_HEIGHT);

         // The initial vertical and horizontal direction of the ball randomly.
         RandomizeBallDirection();

         // The initial speed of the ball.
         mBallSpeed = PongConstants.BALL_INITIAL_SPEED;

         // Indicate the painting callback method to use when a repaint is called.
         this.Paint += Draw;

         // Indicate that key events will be used in this window.
         this.KeyPreview = true;
         // Indicate the callback method to use when a key is pressed down.
         this.KeyDown += PongKeyDown;
         // Indicate the callback method to use when a key is released.
         this.KeyUp += PongKeyUp;

         // Start up the periodic timer.
         mTimer = new Timer();
         mTimer.Tick += new EventHandler(TimerTick);
         mTimer.Interval = PongConstants.TIMER_INTERVAL;
         mTimer.Enabled = true;
         mTimer.Start();
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: NewMatch
      //
      // Description:
      //  Reset game components when. Randomize the horizontal and vertical direction of the ball when the match begins. Reset the ball speed to its
      //  initial speed. Put the game into the prematch wait state.
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void NewMatch()
      {
         // Reset the paddles to starting positions.
         mLeftPaddle.Y = (this.Size.Height / PongConstants.HALF) - (PongConstants.PADDLE_HEIGHT / PongConstants.HALF);
         mRightPaddle.Y = (this.Size.Height / PongConstants.HALF) - (PongConstants.PADDLE_HEIGHT / PongConstants.HALF);

         // Reset the ball to be the center of the screen.
         mBall.X = (this.Size.Width / PongConstants.HALF) - (PongConstants.BALL_WIDTH_AND_HEIGHT / PongConstants.HALF);
         mBall.Y = (this.Size.Height / PongConstants.HALF) - (PongConstants.BALL_WIDTH_AND_HEIGHT / PongConstants.HALF);

         // Set the horizontal and vertical direction of the ball randomly.
         RandomizeBallDirection();

         // Reset the ball speed.
         mBallSpeed = PongConstants.BALL_INITIAL_SPEED;

         // Start the match in the prematch wait state to allow players to view the current score and prepare for the next match.
         mPrematchWait = true;
         mPrematchTime = PongConstants.PREMATCH_WAIT_TIME;  
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: RandomizeBallDirection
      //
      // Description:
      //  Randomize the vertical and horizontal direction of the ball when called.
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void RandomizeBallDirection()
      {
         // Create a new instance of random to allow a pseudo random number.
         Random random = new Random();

         // Randomize the vertical direction.
         Array VerticalDirectionValues = Enum.GetValues(typeof(VerticalDirection));
         mBallVerticalDirection = (VerticalDirection)VerticalDirectionValues.GetValue(random.Next(VerticalDirectionValues.Length));

         // Randomize the horizontal direction.
         Array HorizontalDirectionValues = Enum.GetValues(typeof(HorizontalDirection));
         mBallHorizontalDirection = (HorizontalDirection)HorizontalDirectionValues.GetValue(random.Next(HorizontalDirectionValues.Length));
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: PongLoad
      //
      // Description:
      //  As it stands now this is method is needed to run the program, but has no implementation.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void PongLoad(object theSender, EventArgs theEventArguments)
      {
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: PongKeyDown
      //
      // Description:
      //  Determine which key was pressed down:
      //      W - Lets the program to know to move the left paddle up on the next update.
      //      S - Lets the program to know to move the left paddle down on the next update.
      //      O - Lets the program to know to move the right paddle up on the next update.
      //      L - Lets the program to know to move the right paddle down on the next update.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void PongKeyDown(Object theSender, KeyEventArgs theEventArguments)
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
      // Method Name: PongKeyUp
      //
      // Description:
      //  Determine which key was release:
      //      W - Lets the program to know to no longer move the left paddle up on the next update.
      //      S - Lets the program to know to no longer move the left paddle down on the next update.
      //      O - Lets the program to know to no longer move the right paddle up on the next update.
      //      L - Lets the program to know to no longer move the right paddle down on the next update.
      //
      // Arguments:
      //  theSender - The object that sent the event arguments.
      //  theEventArguments - The events that occurred by the sender.
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void PongKeyUp(Object theSender, KeyEventArgs theEventArguments)
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
      //  Call to update the objects in the game based on the current game state.
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
         // Check is in the prematch wait state.
         if (mPrematchWait == false)
         {
            // Update the paddles on the window based on the keys pressed down or released.
            UpdatePaddle(mLeftMoveUp,
                         mLeftMoveDown,
                         ref mLeftPaddle);
            UpdatePaddle(mRightMoveUp,
                         mRightMoveDown,
                         ref mRightPaddle);
            UpdateBall();
            CheckBallCollision();
         }
         else
         {
            // Reduce the wait time by the timer interval.
            mPrematchTime -= PongConstants.TIMER_INTERVAL;

            // Check to see if the prematch time has elapsed.
            if (mPrematchTime < 0)
            {
               // Since the time has elapsed, the game is no longer in the prematch wait state.
               mPrematchWait = false;
            }
         }
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: UpdatePaddle
      //
      // Description:
      //  Call to update the paddle positions in the game based on the up and down buttons being pressed.
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
         // Note: If both up AND down buttons are pressed the up button takes precedence.
         if (theUp == true)
         {
            // Set the paddle y-coordinate to draw further up the window by the paddle speed.
            thePaddle.Y -= PongConstants.PADDLE_SPEED;

            // Prevent the paddle from going out of bounds at the top of the window.
            if(thePaddle.Y < PongConstants.SCREEN_Y_COORDINATE_TOP)
            {
               thePaddle.Y = PongConstants.SCREEN_Y_COORDINATE_TOP;
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
      // Method Name: UpdateBall
      //
      // Description:
      //  Update the ball based on its vertical direction, horizontal direction and current ball speed.
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void UpdateBall()
      {
         // Increase or decrease the y-coordinate of the ball based on the ball speed and vertical direction of the ball.
         switch (mBallVerticalDirection)
         {
            // Decrease the y-coordinate (draw higher on the screen) by the ball speed if the vertical direction is upwards.
            case VerticalDirection.UP:
            {
               mBall.Y -= mBallSpeed;
               break;
            }
            // Increase the y-coordinate (draw lower on the screen) by the ball speed if the vertical direction is downwards.
            case VerticalDirection.DOWN:
            {
               mBall.Y += mBallSpeed;
               break;
            }
            // Reset to a new match if the vertical direction is unknown.
            default:
            {
               NewMatch();
               break;
            }
         }

         // Increase or decrease the x-coordinate of the ball based on the ball speed and horizontal direction of the ball.
         switch (mBallHorizontalDirection)
         {
            // Decrease the y-coordinate (draw more to the left on the screen) by the ball speed if the horizontal direction is to the left.
            case HorizontalDirection.LEFT:
            {
               mBall.X -= mBallSpeed;
               break;
            }
            // Increase the y-coordinate (draw more to the right on the screen) by the ball speed if the horizontal direction is to the right.
            case HorizontalDirection.RIGHT:
            {
               mBall.X += mBallSpeed;
               break;
            }
            // Reset to a new match if the horizontal direction is unknown.
            default:
            {
               NewMatch();
               break;
            }
         }
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: CheckBallCollision
      //
      // Description:
      //  Check collision of the ball against the top, bottom, left, and right boundaries as well as the ball against the paddles. The ball will
      //  change in reverse horizontal direction when collision against a paddle and change reverse vertical direction when collision against the
      //  top and bottom boundaries. A ball collision against the
      //
      // Arguments:
      //  N/A
      //
      // Return:
      //  N/A
      //
      //*********************************************************************************************************************************************
      private void CheckBallCollision()
      {
         // Check ball collision against the top boundary and reverse the vertical direction to be downwards if the boundary is hit.
         // Increase the ball speed if collision occurs.
         if (mBall.Y < 0)
         {
            mBallVerticalDirection = VerticalDirection.DOWN;
            
            mBallSpeed += PongConstants.BALL_SPEED_INCREASE;
            if (mBallSpeed > PongConstants.BALL_MAXIMUM_SPEED)
            {
               mBallSpeed = PongConstants.BALL_MAXIMUM_SPEED;
            }
         }
         // Check ball collision against the bottom boundary and reverse the vertical direction to be upwards if the boundary is hit.
         // Increase the ball speed if collision occurs.
         else if (mBall.Y > (this.Size.Height - PongConstants.BALL_WIDTH_AND_HEIGHT))
         {
            mBallVerticalDirection = VerticalDirection.UP;

            mBallSpeed += PongConstants.BALL_SPEED_INCREASE;
            if (mBallSpeed > PongConstants.BALL_MAXIMUM_SPEED)
            {
               mBallSpeed = PongConstants.BALL_MAXIMUM_SPEED;
            }
         }

         // Check ball collision against the left paddle and reverse the horizontal direction to go right if the paddle is hit.
         // Increase the ball speed if collision occurs.
         if (mBall.IntersectsWith(mLeftPaddle))
         {
            mBallHorizontalDirection = HorizontalDirection.RIGHT;

            mBallSpeed += PongConstants.BALL_SPEED_INCREASE;
            if (mBallSpeed > PongConstants.BALL_MAXIMUM_SPEED)
            {
               mBallSpeed = PongConstants.BALL_MAXIMUM_SPEED;
            }
         }
         // Check ball collision against the right paddle and reverse the horizontal direction to go left if the paddle is hit.
         // Increase the ball speed if collision occurs.
         else if (mBall.IntersectsWith(mRightPaddle))
         {
            mBallHorizontalDirection = HorizontalDirection.LEFT;

            mBallSpeed += PongConstants.BALL_SPEED_INCREASE;
            if (mBallSpeed > PongConstants.BALL_MAXIMUM_SPEED)
            {
               mBallSpeed = PongConstants.BALL_MAXIMUM_SPEED;
            }
         }

         // Check ball collision against the left boundary.
         // Note: The checks make sure the ball is completely off screen before registering.
         if (mBall.X < (0 - mBall.Width))
         {
            // The right player scores, begin a new match.
            mRightPlayerScore++;
            NewMatch();
         }
         // Check ball collision against the right boundary.
         // Note: The checks make sure the ball is completely off screen before registering.
         else if (mBall.X > this.Size.Width)
         {
            // The left player scores, begin a new match.
            mLeftPlayerScore++;
            NewMatch();
         }
      }

      //*********************************************************************************************************************************************
      //
      // Method Name: Draw
      //
      // Description:
      //  Draw the game objects onto the window.
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
         SolidBrush paddleColor = new SolidBrush(Color.Green);
         SolidBrush ballColor = new SolidBrush(Color.Black);

         // Draw the paddle and balls onto the window.
         theEventArguments.Graphics.FillRectangle(paddleColor,
                                                  mLeftPaddle);
         theEventArguments.Graphics.FillRectangle(paddleColor,
                                                  mRightPaddle);
         theEventArguments.Graphics.FillRectangle(ballColor,
                                                  mBall);

         // While in prematch wait state, display the player scores and the timer for when the next match starts.
         if (mPrematchWait == true)
         {
            // Setup the text for the scores and timers.
            Font textFont = new System.Drawing.Font(PongConstants.TEXT_FAMILY_NAME,
                                                    PongConstants.TEXT_SIZE);
            SolidBrush textColor = new SolidBrush(Color.Black);
            StringFormat textFormat = new StringFormat();
            textFormat.Alignment = StringAlignment.Center;
            textFormat.LineAlignment = StringAlignment.Center;

            // Draw the left player score to be center on the left half (first quarter horizontal, half vertical) of the screen.
            theEventArguments.Graphics.DrawString(PongConstants.LEFT_PLAYER_SCORE_TEXT + mLeftPlayerScore.ToString(),
                                                  textFont,
                                                  textColor,
                                                  this.Size.Width / PongConstants.QUARTER,
                                                  this.Size.Height / PongConstants.HALF,
                                                  textFormat);

            // Draw the right player score to be centered on the right half (third quarter horizontal, half vertical) of the screen.
            theEventArguments.Graphics.DrawString((PongConstants.RIGHT_PLAYER_SCORE_TEXT + mRightPlayerScore.ToString()),
                                                  textFont,
                                                  textColor,
                                                  (this.Size.Width / PongConstants.HALF) + (this.Size.Width / PongConstants.QUARTER),
                                                  this.Size.Height / PongConstants.HALF,
                                                  textFormat);

            // Draw the countdown timer for the prematch wait state to the center (half horizontal, first quarter vertical) of the screen.
            // Note: The timer will display as an integer in seconds.
            int timerInSeconds = (mPrematchTime / PongConstants.MILLISECONDS_IN_SECONDS) + PongConstants.INTEGER_DIVISION_CIELING;
            theEventArguments.Graphics.DrawString(timerInSeconds.ToString(),
                                                  textFont,
                                                  textColor,
                                                  this.Size.Width / PongConstants.HALF,
                                                  this.Size.Height / PongConstants.QUARTER,
                                                  textFormat);

            // Clean up allocated memory.
            textFont.Dispose();
            textColor.Dispose();
            textFormat.Dispose();
         }

         // Clean up allocated memory.
         paddleColor.Dispose();
         ballColor.Dispose();
      }
   }
}
