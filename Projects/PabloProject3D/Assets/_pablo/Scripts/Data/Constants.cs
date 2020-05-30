namespace Pablo
{
    public static class Constants
    {

      public enum Scene
      {
        None,
        Init,
        Menu,
        Game,
        Debug
      }

      public enum PlayMode
      {
        Play,
        Blocked, // UI
        Stop,
        Exiting,
      }


      public enum Requester
      {
        None,
        Monigote,
        Personaje,
        Transeunte,
        Paisano,
        Menganito
      }

      public enum Item
      {
        None,
        Manzana,
        Chorizo,
        Hogaza,
        Bocata,
        Abuela,
      }

      public enum Map
      {
        None,
        Debug0,
        Debug1,
        Debug2,
      }


      public enum Direction2Axis
      {
        Left,
        Right
      }

      public enum Direction4Axis
      {
        Up,
        Down,
        Left,
        Right,
      }

    }
}