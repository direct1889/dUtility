
using UnityEngine;

using System.Collections.Generic;
using System.Linq;



namespace du.di.Id {


    public enum Player {
        _1P, _2P, _3P, _4P, Any
    }

    public enum GamePad {
        _1P, _2P, _3P, _4P, Any
    }

    public enum GamePadRaw {
        Any, _1P, _2P, _3P, _4P, Index_Max
    }

    public static class IdConverter {


        //! ----- field -----
        #region field

        static Dictionary<Player, GamePad> player2gamepad
            = new Dictionary<Player, GamePad>();

        static Dictionary<GamePad, GamePadRaw> gamepad2raw
            = new Dictionary<GamePad, GamePadRaw>();

        #endregion


        //! ----- getter -----
        #region getter

        public static GamePad ToGamePadID(Player playerID) {
            return player2gamepad[playerID];
        }
        public static GamePadRaw ToRawID(GamePad gpID) {
            try {
                return gamepad2raw[gpID];
            }
            catch (System.Exception e) {
                Debug.LogError(e);
                return GamePadRaw.Any;
            }
        }

        #endregion


        //! ----- public -----
        #region public

        public static void SetPlayer2GamePad(Player playerID, GamePad gpID) {
            if (player2gamepad.ContainsKey(playerID)) {
                player2gamepad[playerID] = gpID;
            }
            else {
                player2gamepad.Add(playerID, gpID);
            }
        }

        public static void ResetPlayer2GamePad() {
            player2gamepad.Clear();
            player2gamepad.Add(Player._1P, GamePad._1P);
            player2gamepad.Add(Player._2P, GamePad._2P);
            player2gamepad.Add(Player._3P, GamePad._3P);
            player2gamepad.Add(Player._4P, GamePad._4P);
            player2gamepad.Add(Player.Any, GamePad.Any);
        }

        public static void SetPlayer2GamePad(
            GamePad _1p, GamePad _2p, GamePad _3p, GamePad _4p)
        {
            player2gamepad.Clear();
            player2gamepad.Add(Player._1P, _1p);
            player2gamepad.Add(Player._2P, _2p);
            player2gamepad.Add(Player._3P, _3p);
            player2gamepad.Add(Player._4P, _4p);
            player2gamepad.Add(Player.Any, GamePad.Any);
        }


        private class RawGamePadWrapper {

            [File.CSVColAttr(0)] GamePadRaw raw1P;
            [File.CSVColAttr(1)] GamePadRaw raw2P;
            [File.CSVColAttr(2)] GamePadRaw raw3P;
            [File.CSVColAttr(3)] GamePadRaw raw4P;

            public void Register(Dictionary<GamePad, GamePadRaw> gp2raw) {

                Debug.Log(string.Format("1P:{0}, 2P:{1}, 3P:{2}, 4P:{3}",
                    raw1P, raw2P, raw3P, raw4P));
                gp2raw.Clear();
                gp2raw.Add(GamePad._1P, raw1P);
                gp2raw.Add(GamePad._2P, raw2P);
                gp2raw.Add(GamePad._3P, raw3P);
                gp2raw.Add(GamePad._4P, raw4P);
                gp2raw.Add(GamePad.Any, GamePadRaw.Any);

            }

            public override string ToString() {
                return "size = " + player2gamepad.Count;
            }

        }

        public static void Initialize() {

            ResetPlayer2GamePad();

            using (var rawGPWrapperReader =
                new File.CSVReader<RawGamePadWrapper>
                ("Utility/RawGamePadWrapper", true, false)
                )
            {
                rawGPWrapperReader.ToList().ForEach(
                    i => i.Register(gamepad2raw)
                );
            }

        }

        public static void PrintLog() {

            string rtn = "";
            for (Player i = Player._1P; i <= Player.Any; ++i) {
                rtn += string.Format("Player : {0} : {1} : {2} : RawGP\n",
                    i, i.ToGPID(), i.ToGPID().ToRawID());
            }
            Debug.Log(rtn);

        }

        #endregion


    }


    //! IDに拡張メソッドを提供する
    public static class IdExtension {

        public static int ToInt(this Player player) {
            return player.ToInt0Origin() + 1;
        }
        public static int ToInt0Origin(this Player player) {
            return (int)player;
        }
        public static Player ToPlayerID(this int id) {
            switch (id) {
                case 1 : return Player._1P;
                case 2 : return Player._2P;
                case 3 : return Player._3P;
                case 4 : return Player._4P;
                default: return Player.Any;
            }
        }
        public static Player ToPlayerID0Origin(this int id0Origin) {
            return (id0Origin + 1).ToPlayerID();
        }

        public static Color ToColor(this Player player) {
            switch (player) {
                case Player._1P: return Color.red;
                case Player._2P: return Color.blue;
                case Player._3P: return Color.yellow;
                case Player._4P: return Color.green;
                default        : return Color.black;
            }
        }
        public static string ToColorString(this Player player) {
            switch (player) {
                case Player._1P: return "Red";
                case Player._2P: return "Blue";
                case Player._3P: return "Yellow";
                case Player._4P: return "Green";
                default        : return "Null";
            }
        }

        public static bool In(this Player own, params Player[] others) {
            return others.Contains(own);
        }


        public static GamePad ToGPID(this Player playerID) {
            return IdConverter.ToGamePadID(playerID);
        }
        public static GamePadRaw ToRawID(this Player playerID) {
            return IdConverter.ToRawID(IdConverter.ToGamePadID(playerID));
        }
        public static GamePadRaw ToRawID(this GamePad gpID) {
            return IdConverter.ToRawID(gpID);
        }

    }


    //! IDに関する頻出メソッドを提供する
    public static class Util {


        public static class PlayerID {

            public static IList<Player> GetValids() {
                return new Player[4] {
                    Player._1P, Player._2P,
                    Player._3P, Player._4P
                };
            }

        }

        public static class GamePadID {

            public static IList<GamePad> GetValids() {
                return new GamePad[4] {
                    GamePad._1P, GamePad._2P,
                    GamePad._3P, GamePad._4P
                };
            }

        }


    }

}
