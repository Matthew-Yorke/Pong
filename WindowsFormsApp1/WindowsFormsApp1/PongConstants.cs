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

      // The y-coordinate of the top of the screen.
      public const int SCREEN_Y_COORDINATE_TOP= 0;

      // The speed (number of pixel movement) any paddle in the game can move at.
      public const int PADDLE_SPEED = 10;

      // THe width and height in pixels the ball in the game is.
      public const int BALL_WIDTH_AND_HEIGHT = 20;

      // The number of milliseconds for a clock tick to occur.
      public const int TIMER_INTERVAL = 16;

      // The number of milliseconds for the prematch wait state to be in.
      public const int PREMATCH_WAIT_TIME = 5000;

      // the number of milliseconds in a second.
      public const int MILLISECONDS_IN_SECONDS = 1000;

      // Add this number to an integer division to round the number up.
      public const int INTEGER_DIVISION_CIELING = 1;

      // Used for dividing a value in half.
      public const int HALF = 2;

      // Used for dividing a value to a quarter.
      public const int QUARTER = 4;

      // The inital speed of a ball when a match begins.
      public const int BALL_INITIAL_SPEED = 2;

      // The speed increase when the ball hits a boundary or paddle.
      public const int BALL_SPEED_INCREASE = 1;

      // The maximum speed the ball can travel.
      public const int BALL_MAXIMUM_SPEED = 15;

      // The initial score of a player in a new game.
      public const int PLAYER_INITIAL_SCORE = 0;

      // The family name for the score and timer text.
      public const string TEXT_FAMILY_NAME = "Arial";

      // The font size for the score and timer text.
      public const int TEXT_SIZE = 32;

      // The text to indicate the number on the left during the prematch wait state is the player 1 score.
      public const string LEFT_PLAYER_SCORE_TEXT = "Player 1\n";

      // The text to indicate the number on the right during the prematch wait state is the player 1 score.
      public const string RIGHT_PLAYER_SCORE_TEXT = "Player 2\n";
   }
}
