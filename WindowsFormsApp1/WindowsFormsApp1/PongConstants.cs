//************************************************************************************************************************************************
//
// File Name: PongConstants.cs
//
// Description:
//  Constants used throughout the pong game.
//
// Change History:
//  Author               Date           Description
//  Matthew D. Yorke     12/22/2017     Initial set of constants before adding to github to keep as a backup.
//
//************************************************************************************************************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
   public static class PongConstants
   {
      // The width in pixels of any paddle in the game.
      public const int PADDLE_WIDTH = 10;

      // The height in pixels of any paddle in the game
      public const int PADDLE_HEIGHT = 200;

      // The padding in pixels between a endzone and any paddle in the game.
      public const int PADDLE_BOUNDARY_PADDING = 20;

      // The stareting y-coordinate in pixels for any paddle in the game.
      public const int PADDLE_STARTING_Y_COORDINATE = 0;

      // The speed (number of pixel movement) any paddle in the game can move at.
      public const int PADDLE_SPEED = 10;

      // THe width and height in pixels the ball in the game is.
      public const int BALL_WIDTH_AND_HEIGHT = 20;

      // THe number of milliseconds for a clock tick to occur.
      public const int TIMER_INTERVAL = 16;

      // Used for dividing a value in half.
      public const int HALF = 2;

      // The inital speed of a ball when a match begins.
      public const int BALL_INITIAL_SPEED = 2;

      // The speed increase when the ball hits a boundary or paddle.
      public const int BALL_SPEED_INCREASE = 1;

      // The maximum speed the ball can travel.
      public const int BALL_MAXIMUM_SPEED = 15;
   }
}
