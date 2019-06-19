using UnityEngine;

using di = du.di;
using PlayerID = du.di.Id.Player;


namespace Bsc {

    enum Direction {

        Left, Right, Up, Down, Front, Back, Max

    }

}


namespace Cmp {


    interface ISimpleMove {

        Vector3 Factor { get; set; }

        Vector3 MoveVector();

    }


    public class SimpleMove4GP : ISimpleMove {

        bool m_isXZ;

        public Vector3 Factor { get; set; }
        public PlayerID Player { get; private set; }

        //! if !isXZ then isXY
        public SimpleMove4GP(PlayerID playerID, Vector3 factor, bool isXZ = false) {
            Factor = factor;
            Player = playerID;
            m_isXZ = isXZ;
        }

        public Vector3 MoveVector() {
            if (m_isXZ) {
                return new Vector3(
                    di.GamePad.GetLeftAxis(Player).x * Factor.x,
                    0,
                    di.GamePad.GetLeftAxis(Player).y * Factor.z
                    );
            }
            else {
                return du.Ex.ExVector.ElemProduct(
                    di.GamePad.GetLeftAxis(Player),
                    Factor);
            }
        }

    }


#if false

    public class SimpleMove : ISimpleMove
    {

        readonly Dictionary<Bsc.Direction, InputMode?>
            m_dirKeyMap = new Dictionary<Bsc.Direction, InputMode?>();

        readonly Game.Player m_player;

        public Vector3 Factor { get; set; }


        //! x方向 : left/right, y方向 : up/down, z方向 : front/back
        public SimpleMove(
            Game.Player playerID,
            InputMode? left, InputMode? right,
            InputMode? up, InputMode? down,
            InputMode? front, InputMode? back,
            Vector3 factor)
        {
            m_player = playerID;
            m_dirKeyMap.Add(Bsc.Direction.Left, left);
            m_dirKeyMap.Add(Bsc.Direction.Right, right);
            m_dirKeyMap.Add(Bsc.Direction.Up, up);
            m_dirKeyMap.Add(Bsc.Direction.Down, down);
            m_dirKeyMap.Add(Bsc.Direction.Front, front);
            m_dirKeyMap.Add(Bsc.Direction.Back, back);
            Factor = factor;
        }

        public SimpleMove(
            Game.Player playerID,
            InputMode? left, InputMode? right,
            InputMode? up, InputMode? down,
            InputMode? front, InputMode? back)
            : this(playerID, left, right, up, down, front, back, Vector3.one) { }


        public Vector3 MoveVector()
        {
            Vector3 moveDir = Vector3.zero;
            moveDir.x = CounterDir2Value(
                m_player,
                m_dirKeyMap[Bsc.Direction.Right],
                m_dirKeyMap[Bsc.Direction.Left]
                ) * Factor.x;
            moveDir.y = CounterDir2Value(
                m_player,
                m_dirKeyMap[Bsc.Direction.Down],
                m_dirKeyMap[Bsc.Direction.Up]
                ) * Factor.y;
            moveDir.z = CounterDir2Value(
                m_player,
                m_dirKeyMap[Bsc.Direction.Front],
                m_dirKeyMap[Bsc.Direction.Back]
                ) * Factor.z;
            return moveDir;
        }


        /*! 相反する2方向を表すキーを受け取り、移動方向を返す
            @param[in]  (右, 左), (下, 上), (前方, 後方)など
            @return     e.g.CounterDir2Value(右キー, 左キー)
                右キーのみ押下 :  1
                左キーのみ押下 : -1
                両キーとも押下 :  0
        */
        private static float CounterDir2Value(
            Game.Player index,
            InputMode? positiveDir,
            InputMode? negativeDir)
        {
            return InputManager.Get(positiveDir, index)
                - InputManager.Get(negativeDir, index);
        }

    }

#endif


}
