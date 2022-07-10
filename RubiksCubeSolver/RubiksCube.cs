using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    /// <summary>
    /// Holds the data 
    /// </summary>
    public class RubiksCube
    {
        /// <summary>
        /// All possible moves you can do on a cube.
        /// There are more moves here than will be allowed for
        /// a standard most efficient solve.
        /// </summary>
        private enum eMoves
        {
            U, D, R, L, F, B,
            UP, DP, RP, LP, FP, BP,
            U2, D2, R2, L2, F2, B2,
            Uw, Dw, Rw, Lw, Fw, Bw,
            UwP, DwP, RwP, LwP, FwP, BwP,
            Uw2, Dw2, Rw2, Lw2, Fw2, Bw2,
            M, E, S,
            MP, EP, SP,
            M2, E2, S2,
            NO_MOVE
        }

        const int TOP_FACE = 0;
        const int BOTTOM_FACE = 1;
        const int FRONT_FACE = 2;
        const int BACK_FACE = 3;
        const int LEFT_FACE = 4;
        const int RIGHT_FACE = 5;

        const int Y_AXIS = 0;
        const int X_AXIS = 1;
        const int Z_AXIS = 2;

        private char[,] mState = new char[6, 9];

        private Stack<eMoves> mScamble = new Stack<eMoves>();
        private Stack<eMoves> mSolve = new Stack<eMoves>();

        /// <summary>
        /// The set of moves that can be used for scambling and
        /// a most efficient solve.
        /// </summary>
        private IReadOnlyList<eMoves> mStandardMoveset = new List<eMoves>()
        {
            eMoves.U, eMoves.D, eMoves.R, eMoves.L, eMoves.F, eMoves.B,
            eMoves.UP, eMoves.DP, eMoves.RP, eMoves.LP, eMoves.FP, eMoves.BP,
            eMoves.U2, eMoves.D2, eMoves.R2, eMoves.L2, eMoves.F2, eMoves.B2,
            eMoves.M, eMoves.E, eMoves.S,
            eMoves.MP, eMoves.EP, eMoves.SP,
            eMoves.M2, eMoves.E2, eMoves.S2,

            // To be removed.
            eMoves.Rw, eMoves.Rw2, eMoves.RwP,
            eMoves.Lw, eMoves.Lw2, eMoves.RwP,
            eMoves.Fw, eMoves.Fw2, eMoves.FwP,
            eMoves.Bw, eMoves.Bw2, eMoves.BwP,
        };

        /// <summary>
        /// Enumerations are integers, yes.
        /// Maps each move to its inverse.
        /// </summary>
        private IReadOnlyList<eMoves> mReverseMoves = new eMoves[(int)eMoves.NO_MOVE]
        {
            eMoves.UP, eMoves.DP, eMoves.RP, eMoves.LP, eMoves.FP, eMoves.BP,
            eMoves.U, eMoves.D, eMoves.R, eMoves.L, eMoves.F, eMoves.B,
            eMoves.U2, eMoves.D2, eMoves.R2, eMoves.L2, eMoves.F2, eMoves.B2,
            eMoves.UwP, eMoves.DwP, eMoves.RwP, eMoves.LwP, eMoves.FwP, eMoves.BwP,
            eMoves.Uw, eMoves.Dw, eMoves.Rw, eMoves.Lw, eMoves.Fw, eMoves.Bw,
            eMoves.Uw2, eMoves.Dw2, eMoves.Rw2, eMoves.Lw2, eMoves.Fw2, eMoves.Bw2,
            eMoves.MP, eMoves.EP, eMoves.SP,
            eMoves.M, eMoves.E, eMoves.S,
            eMoves.M2, eMoves.E2, eMoves.S2,
        };

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public RubiksCube()
        {
            Reset();

            // Create face order rotation arrays.
            mReverseRowRotationFaceOrder = Enumerable.Reverse(mRowRotationFaceOrder).ToArray();
            mReverseLeftRightFaceOrder = Enumerable.Reverse(mLeftRightFaceOrder).ToArray();
            mReverseFrontBackFaceOrder = Enumerable.Reverse(mFrontBackFaceOrder).ToArray();
        }

        public void Reset()
        {
            // Yellow on top, white on bottom, blue in front, green in back, 
            // orange on the left, and red on the right.
            char[] colors = new char[6] { 'y', 'w', 'b', 'g', 'o', 'r' };

            for(int i = 0; i < colors.Length; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    mState[i, j] = colors[i];
                }
            }

            mScamble.Clear();
            mSolve.Clear();
        }

        /// <summary>
        /// Scrambles the cube by creating a sequence of random moves 
        /// within the list of standard moves.
        /// </summary>
        /// <param name="numMoves"></param>
        public void Scramble(int numMoves = 30)
        {
            Random random = new Random();

            for(int i = 0; i < numMoves; i++)
            {
                int randomIndex = random.Next(mStandardMoveset.Count());
                eMoves randomMove = mStandardMoveset[randomIndex];
                PushMove(randomMove, mScamble);
            }
        }

        /// <summary>
        /// Adds a move to the cube pushing it onto a specific stack.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="moveStack"></param>
        private void PushMove(eMoves move, Stack<eMoves> moveStack)
        {
            moveStack.Push(move);
            HandleMove(move);
        }

        /// <summary>
        /// Reverses a move from a given move stack.
        /// </summary>
        /// <param name="moveStack"></param>
        private void PopMove(Stack<eMoves> moveStack)
        {
            if(moveStack.Count > 0)
            {
                eMoves inverseMove = mReverseMoves[(int)moveStack.Pop()];
                HandleMove(inverseMove);
            }
        }

        /// <summary>
        /// Solves the rubiks cube the intuitive way.
        /// </summary>
        public void SolveAlgorithmicly()
        {

        }

        /// <summary>
        /// Solves the cube in the fewest moves.
        /// Done by performing each possible move recursively until
        /// it finds the first move that solves the cube.
        /// </summary>
        public void MostEfficientSolve()
        {

        }

        // TOP-BOTTOM-FRONT-BACK-LEFT-RIGHT.
        private IReadOnlyList<IReadOnlyList<IReadOnlyList<IReadOnlyList<int>>>> mIndexMappings = new int[][][][]
        {
            // YAxis.
            new int[][][]
            {
                // Top to X. Rotating the top face along the Y axis can never get to any face other than the top.
                // The top face would remain unchanged.
                new int[][]
                {
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Top to Top.
                    new int[] { -1 },                          // Top to Bottom. NOT POSSIBLE.
                    new int[] { -1 },                          // Top to Front. NOT POSSIBLE.
                    new int[] { -1 },                          // Top to Back. NOT POSSIBLE.
                    new int[] { -1 },                          // Top to Left. NOT POSSIBLE.
                    new int[] { -1 },                          // Top to Right. NOT POSSIBLE.
                },
                // Bottom to X.
                // The bottom face cannot get anywhere except the bottom face as well.
                new int[][]
                {
                    new int[] { -1 },                          // Bottom to Bottom. NOT POSSIBLE.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Bottom to Bottom.
                    new int[] { -1 },                          // Bottom to Front. NOT POSSIBLE.
                    new int[] { -1 },                          // Bottom to Back. NOT POSSIBLE.
                    new int[] { -1 },                          // Bottom to Left. NOT POSSIBLE.
                    new int[] { -1 },                          // Bottom to Right. NOT POSSIBLE.
                },
                // Front to X.
                new int[][]
                {
                    new int[] { -1 },                          // Front to Top. NOT POSSIBLE.
                    new int[] { -1 },                          // Front to Bottom. NOT POSSIBLE.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Front to Front.
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },  // Front to Back.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Front to Left.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Front to Right.
                },
                // Back to X.
                new int[][]
                {
                    new int[] { -1 },                          // Back to Top. NOT POSSIBLE.
                    new int[] { -1 },                          // Back to Bottom. NOT POSSIBLE.
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },  // Back to Front.  
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Back to Back.  
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },  // Back to Left.  
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },  // Back to Right.  
                },
                // Left to X.
                new int[][]
                {
                    new int[] { -1 },                          // Left to Top. NOT POSSIBLE.
                    new int[] { -1 },                          // Left to Bottom. NOT POSSIBLE.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Left to Front.
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },  // Left to Back.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Left to Left.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Left to Right.
                },
                // Right to X.
                new int[][]
                {
                    new int[] { 0 },                          // Right to Top. NOT POSSIBLE.
                    new int[] { 0 },                          // Right to Bottom. NOT POSSIBLE.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Right to Front.
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },  // Right to Back.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Right to Left.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },  // Right to Right.
                }
            },

            // XAxis.
            new int[][][]
            {
                // Top to X.
                new int[][]
                {
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Top to Top.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Top to Bottom.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Top to Front.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Top to Back.
                    new int[] { -1 },                             // Top to Left. NOT POSSIBLE.
                    new int[] { -1 },                             // Top to Right. NOT POSSIBLE.
                },
                // Bottom to X.
                new int[][]
                {
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Bottom to Top.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Bottom to Bottom.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Bottom to Front.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Bottom to Back.
                    new int[] { -1 },                             // Bottom to Left.
                    new int[] { -1 },                             // Bottom to Right.
                },
                // Front to X.
                new int[][]
                {
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Front to Top.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Front to Bottom.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Front to Front.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Front to Back.
                    new int[] { -1 },                             // Front to Left.
                    new int[] { -1 },                             // Front to Right.
                },
                // Back to X.
                new int[][]
                {
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Back to Top.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Back to Bottom.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Back to Front.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Back to Back.
                    new int[] { -1 },                             // Back to Left.
                    new int[] { -1 },                             // Back to Right.
                },
                // Left to X.
                new int[][]
                {
                    new int[] { -1 },                             // Left to Top.
                    new int[] { -1 },                             // Left to Bottom.
                    new int[] { -1 },                             // Left to Front.
                    new int[] { -1 },                             // Left to Back.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Left to Left.
                    new int[] { -1 },                             // Left to Right.
                },
                // Right to X.
                new int[][]
                {
                    new int[] { -1 },                             // Right to Top.
                    new int[] { -1 },                             // Right to Bottom.
                    new int[] { -1 },                             // Right to Front.
                    new int[] { -1 },                             // Right to Back.
                    new int[] { -1 },                             // Right to Left.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },      // Right to Right.
                }
            },

            // ZAxis.
            new int[][][]
            {
                // Top to X.
                new int[][]
                {
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },    // Top to Top.
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },    // Top to Bottom.
                    new int[] { -1 },                           // Top to Front.
                    new int[] { -1 },                           // Top to Back.
                    new int[] { 6, 3, 0, 7, 4, 1, 8, 5, 2 },    // Top to Left.
                    new int[] { 2, 5, 8, 1, 4, 7, 0, 3, 6 },    // Top to Right.
                },
                // Bottom to X.
                new int[][]
                {
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },    // Bottom to Top.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },    // Bottom to Bottom.
                    new int[] { -1 },                           // Bottom to Front.
                    new int[] { -1 },                           // Bottom to Back.
                    new int[] { 2, 5, 8, 1, 4, 7, 0, 3, 6 },    // Bottom to Left.
                    new int[] { 6, 3, 0, 7, 4, 1, 8, 5, 2 },    // Bottom to Right.
                },
                // Front to X.
                new int[][]
                {
                    new int[] { -1 },                           // Front to Top.
                    new int[] { -1 },                           // Front to Bottom.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },    // Front to Front.
                    new int[] { -1 },                           // Front to Back.
                    new int[] { -1 },                           // Front to Left.
                    new int[] { -1 },                           // Front to Right.
                },
                // Back to X.
                new int[][]
                {
                    new int[] { -1 },                           // Back to Top.
                    new int[] { -1 },                           // Back to Bottom.
                    new int[] { -1 },                           // Back to Front.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },    // Back to Back.
                    new int[] { -1 },                           // Back to Left.
                    new int[] { -1 },                           // Back to Right.
                },
                // Left to X.
                new int[][]
                {
                    new int[] { 2, 5, 8, 1, 4, 7, 0, 3, 6 },    // Left to Top.
                    new int[] { 6, 3, 0, 7, 4, 1, 8, 5, 2 },    // Left to Bottom.
                    new int[] { -1 },                           // Left to Front.
                    new int[] { -1 },                           // Left to Back.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },    // Left to Left.
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },    // Left to Right.
                },
                // Right to X.
                new int[][]
                {
                    new int[] { 6, 3, 0, 7, 4, 1, 8, 5, 2 },    // Right to Top.
                    new int[] { 2, 5, 8, 1, 4, 7, 0, 3, 6 },    // Right to Bottom.
                    new int[] { -1 },                           // Right to Front.
                    new int[] { -1 },                           // Right to Back.
                    new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },    // Right to Left.
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },    // Right to Right.
                }
            }
        };

        /// <summary>
        /// Replace the front face with the first item in this list,
        /// then replace each other face with the subsequent one.
        /// </summary>
        private IReadOnlyList<int> mRowRotationFaceOrder = new int[3]
        {
            RIGHT_FACE, BACK_FACE, LEFT_FACE
        };

        private IReadOnlyList<int> mReverseRowRotationFaceOrder;

        /// <summary>
        /// Rotates a vertical layer to the left. 
        /// Right when reverseDirection is true.
        /// Top layer is 0, middle is 1, and bottom is 2.
        /// Call a separate method to rotate the top or bottom face.
        /// U/D rotations.
        /// </summary>
        /// <param name="reverseDirection"></param>
        private void RotateRowEdges(int row, bool reverseDirection)
        {
            int i1 = row * 3 + 0;
            int i2 = row * 3 + 1;
            int i3 = row * 3 + 2;

            IReadOnlyList<int> rotationOrder = reverseDirection ? mReverseRowRotationFaceOrder : mRowRotationFaceOrder;

            RotateEdges(i1, i2, i3, rotationOrder, FRONT_FACE, Y_AXIS);
        }

        /// <summary>
        /// Replace the front face with the first item in this list,
        /// then replace each other face with the subsequent one.
        /// </summary>
        private IReadOnlyList<int> mLeftRightFaceOrder = new int[3]
        {
            TOP_FACE, BACK_FACE, BOTTOM_FACE
        };

        private IReadOnlyList<int> mReverseLeftRightFaceOrder;

        /// <summary>
        /// Columns are ordered from left to right.
        /// Default direction is down.
        /// Handles R/L rotations.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="reverseDirection"></param>
        private void RotateLeftRightColumnEdges(int column, bool reverseDirection)
        {
            int i1 = 0 * 3 + column;
            int i2 = 1 * 3 + column;
            int i3 = 2 * 3 + column;

            IReadOnlyList<int> rotationOrder = reverseDirection ? mReverseLeftRightFaceOrder : mLeftRightFaceOrder;

            RotateEdges(i1, i2, i3, rotationOrder, FRONT_FACE, X_AXIS);
        }

        /// <summary>
        /// Starting with the Top face, each subsequent face in this list
        /// Replaces the last.
        /// </summary>
        private IReadOnlyList<int> mFrontBackFaceOrder = new int[3]
        {
            LEFT_FACE, BOTTOM_FACE, RIGHT_FACE
        };

        private IReadOnlyList<int> mReverseFrontBackFaceOrder;

        /// <summary>
        /// Given a specific column along the z axis, rotate the cube's edges.
        /// Col 0 begins with the back face, and moves towards the front with each step.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="reverseDirection"></param>
        private void RotateBackFrontColumnEdges(int col, bool reverseDirection)
        {
            int i1 = col * 3 + 0;
            int i2 = col * 3 + 1;
            int i3 = col * 3 + 2;

            IReadOnlyList<int> rotationOrder = reverseDirection ? mReverseFrontBackFaceOrder : mFrontBackFaceOrder;

            RotateEdges(i1, i2, i3, rotationOrder, TOP_FACE, Z_AXIS);
        }

        /// <summary>
        /// Rotates just the edges along an axis given an edge rotation order and the three indices
        /// which should be modified by the rotation.
        /// The starting face should be included in the rotation order list.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <param name="i3"></param>
        /// <param name="rotationOrder"></param>
        /// <param name="startingFace"></param>
        /// <param name="axis"></param>
        private void RotateEdges(int i1, int i2, int i3, IReadOnlyList<int> rotationOrder, int startingFace, int axis)
        {
            int faceToSet = startingFace;

            char s1 = mState[faceToSet, i1];
            char s2 = mState[faceToSet, i2];
            char s3 = mState[faceToSet, i3];

            int faceToSeti1 = 0;
            int faceToSeti2 = 0;
            int faceToSeti3 = 0;

            // Move to the current face, from the next face.
            for(int i = 0; i < rotationOrder.Count; i++)
            {
                int nextFace = rotationOrder[i];

                int transformedi1 = mIndexMappings[axis][startingFace][nextFace][i1];
                int transformedi2 = mIndexMappings[axis][startingFace][nextFace][i2];
                int transformedi3 = mIndexMappings[axis][startingFace][nextFace][i3];

                faceToSeti1 = mIndexMappings[axis][nextFace][faceToSet][transformedi1];
                faceToSeti2 = mIndexMappings[axis][nextFace][faceToSet][transformedi2];
                faceToSeti3 = mIndexMappings[axis][nextFace][faceToSet][transformedi3];

                mState[faceToSet, faceToSeti1] = mState[nextFace, transformedi1];
                mState[faceToSet, faceToSeti2] = mState[nextFace, transformedi2];
                mState[faceToSet, faceToSeti3] = mState[nextFace, transformedi3];

                faceToSet = nextFace;
            }

            faceToSeti1 = mIndexMappings[axis][startingFace][faceToSet][i1];
            faceToSeti2 = mIndexMappings[axis][startingFace][faceToSet][i2];
            faceToSeti3 = mIndexMappings[axis][startingFace][faceToSet][i3];

            mState[faceToSet, faceToSeti1] = s1;
            mState[faceToSet, faceToSeti2] = s2;
            mState[faceToSet, faceToSeti3] = s3;
        }

        /// <summary>
        /// Rotates the face clockwise, counterclockwise if reverse is true.
        /// </summary>
        /// <param name="reverseDirection"></param>
        private void RotateFace(int face, bool reverseDirection)
        {
            char corner1 = mState[face, 0];
            char corner2 = mState[face, 2];
            char corner3 = mState[face, 6];
            char corner4 = mState[face, 8];

            char side1 = mState[face, 1];
            char side2 = mState[face, 3];
            char side3 = mState[face, 5];
            char side4 = mState[face, 7];

            // Rotate corners, then edges.
            if(reverseDirection)
            {
                mState[face, 0] = corner2;
                mState[face, 1] = side3;
                mState[face, 2] = corner4;
                mState[face, 3] = side1;

                mState[face, 5] = side4;
                mState[face, 6] = corner1;
                mState[face, 7] = side2;
                mState[face, 8] = corner3;
            }
            else
            {
                mState[face, 0] = corner3;
                mState[face, 1] = side2;
                mState[face, 2] = corner1;
                mState[face, 3] = side4;

                mState[face, 5] = side1;
                mState[face, 6] = corner4;
                mState[face, 7] = side3;
                mState[face, 8] = corner2;
            }
        }

        /// Performs the move on the rubiks cube state.
        /// Yes, just a switch statement handling each type of move one at a time.
        /// </summary>
        /// <param name="move"></param>
        private void HandleMove(eMoves move)
        {
            switch(move)
            {
                case eMoves.U2:
                    RotateFace(TOP_FACE, false);
                    RotateRowEdges(0, false);
                    goto case eMoves.U;
                case eMoves.U:
                    RotateFace(TOP_FACE, false);
                    RotateRowEdges(0, false);
                    break;
                case eMoves.D2:
                    RotateFace(BOTTOM_FACE, false);
                    RotateRowEdges(2, true);
                    goto case eMoves.D;
                case eMoves.D:
                    RotateFace(BOTTOM_FACE, false);
                    RotateRowEdges(2, true);
                    break;
                case eMoves.R2:
                    RotateFace(RIGHT_FACE, false);
                    RotateLeftRightColumnEdges(2, true);
                    goto case eMoves.R;
                case eMoves.R:
                    RotateFace(RIGHT_FACE, false);
                    RotateLeftRightColumnEdges(2, true);
                    break;
                case eMoves.L2:
                    RotateFace(LEFT_FACE, false);
                    RotateLeftRightColumnEdges(0, false);
                    goto case eMoves.L;
                case eMoves.L:
                    RotateFace(LEFT_FACE, false);
                    RotateLeftRightColumnEdges(0, false);
                    break;
                case eMoves.F2:
                    RotateFace(FRONT_FACE, false);
                    RotateBackFrontColumnEdges(2, false);
                    goto case eMoves.F;
                case eMoves.F:
                    RotateFace(FRONT_FACE, false);
                    RotateBackFrontColumnEdges(2, false);
                    break;
                case eMoves.B2:
                    RotateFace(BACK_FACE, false);
                    RotateBackFrontColumnEdges(0, true);
                    goto case eMoves.B;
                case eMoves.B:
                    RotateFace(BACK_FACE, false);
                    RotateBackFrontColumnEdges(0, true);
                    break;

                case eMoves.UP:
                    RotateFace(TOP_FACE, true);
                    RotateRowEdges(0, true);
                    break;
                case eMoves.DP:
                    RotateFace(BOTTOM_FACE, true);
                    RotateRowEdges(2, false);
                    break;
                case eMoves.RP:
                    RotateFace(RIGHT_FACE, true);
                    RotateLeftRightColumnEdges(2, false);
                    break;
                case eMoves.LP:
                    RotateFace(LEFT_FACE, true);
                    RotateLeftRightColumnEdges(0, true);
                    break;
                case eMoves.FP:
                    RotateFace(FRONT_FACE, true);
                    RotateBackFrontColumnEdges(2, true);
                    break;
                case eMoves.BP:
                    RotateFace(BACK_FACE, true);
                    RotateBackFrontColumnEdges(0, false);
                    break;

                case eMoves.Uw2:
                    RotateFace(TOP_FACE, false);
                    RotateRowEdges(0, false);
                    RotateRowEdges(1, false);
                    goto case eMoves.Uw;
                case eMoves.Uw:
                    RotateFace(TOP_FACE, false);
                    RotateRowEdges(0, false);
                    RotateRowEdges(1, false);
                    break;
                case eMoves.Dw2:
                    RotateFace(BOTTOM_FACE, false);
                    RotateRowEdges(1, true);
                    RotateRowEdges(2, true);
                    goto case eMoves.Dw;
                case eMoves.Dw:
                    RotateFace(BOTTOM_FACE, false);
                    RotateRowEdges(1, true);
                    RotateRowEdges(2, true);
                    break;
                case eMoves.Rw2:
                    RotateFace(RIGHT_FACE, false);
                    RotateLeftRightColumnEdges(2, true);
                    RotateLeftRightColumnEdges(1, true);
                    goto case eMoves.Rw;
                case eMoves.Rw:
                    RotateFace(RIGHT_FACE, false);
                    RotateLeftRightColumnEdges(2, true);
                    RotateLeftRightColumnEdges(1, true);
                    break;
                case eMoves.Lw2:
                    RotateFace(LEFT_FACE, false);
                    RotateLeftRightColumnEdges(0, false);
                    RotateLeftRightColumnEdges(1, false);
                    goto case eMoves.Lw;
                case eMoves.Lw:
                    RotateFace(LEFT_FACE, false);
                    RotateLeftRightColumnEdges(0, false);
                    RotateLeftRightColumnEdges(1, false);
                    break;
                case eMoves.Fw2:
                    RotateFace(FRONT_FACE, false);
                    RotateBackFrontColumnEdges(2, false);
                    RotateBackFrontColumnEdges(1, false);
                    goto case eMoves.Fw;
                case eMoves.Fw:
                    RotateFace(FRONT_FACE, false);
                    RotateBackFrontColumnEdges(2, false);
                    RotateBackFrontColumnEdges(1, false);
                    break;
                case eMoves.Bw2:
                    RotateFace(BACK_FACE, false);
                    RotateBackFrontColumnEdges(0, true);
                    RotateBackFrontColumnEdges(1, true);
                    goto case eMoves.Bw;
                case eMoves.Bw:
                    RotateFace(BACK_FACE, false);
                    RotateBackFrontColumnEdges(0, true);
                    RotateBackFrontColumnEdges(1, true);
                    break;

                case eMoves.UwP:
                    RotateFace(TOP_FACE, true);
                    RotateRowEdges(0, true);
                    RotateRowEdges(1, true);
                    break;
                case eMoves.DwP:
                    RotateFace(BOTTOM_FACE, true);
                    RotateRowEdges(1, false);
                    RotateRowEdges(2, false);
                    break;
                case eMoves.RwP:
                    RotateFace(RIGHT_FACE, true);
                    RotateLeftRightColumnEdges(2, false);
                    RotateLeftRightColumnEdges(1, false);
                    break;
                case eMoves.LwP:
                    RotateFace(LEFT_FACE, true);
                    RotateLeftRightColumnEdges(0, true);
                    RotateLeftRightColumnEdges(1, true);
                    break;
                case eMoves.FwP:
                    RotateFace(FRONT_FACE, true);
                    RotateBackFrontColumnEdges(2, true);
                    RotateBackFrontColumnEdges(1, true);
                    break;
                case eMoves.BwP:
                    RotateFace(BACK_FACE, true);
                    RotateBackFrontColumnEdges(0, false);
                    RotateBackFrontColumnEdges(1, false);
                    break;

                case eMoves.M2:
                    RotateLeftRightColumnEdges(1, false);
                    goto case eMoves.M;
                case eMoves.M:
                    RotateLeftRightColumnEdges(1, false);
                    break;
                case eMoves.E2:
                    RotateRowEdges(1, true);
                    goto case eMoves.E;
                case eMoves.E:
                    RotateRowEdges(1, true);
                    break;
                case eMoves.S2:
                    RotateBackFrontColumnEdges(1, false);
                    goto case eMoves.S;
                case eMoves.S:
                    RotateBackFrontColumnEdges(1, false);
                    break;

                case eMoves.MP:
                    RotateLeftRightColumnEdges(1, true);
                    break;
                case eMoves.EP:
                    RotateRowEdges(1, false);
                    break;
                case eMoves.SP:
                    RotateBackFrontColumnEdges(1, true);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Checks if the cube is in a solved position or not.
        /// </summary>
        /// <returns></returns>
        private bool CheckForSolve()
        {
            return false;
        }

        /// <summary>
        /// Prints the cube to the screen
        /// </summary>
        private void PrintCube()
        {
            int[] facePrintOrder = new int[6]
            {
                FRONT_FACE, LEFT_FACE, BACK_FACE, RIGHT_FACE, TOP_FACE, BOTTOM_FACE
            };

            for(int i = 0; i < facePrintOrder.Length; i++)
            {
                PrintFace(facePrintOrder[i]);
            }
        }

        /// <summary>
        /// Text which should be output for each face.
        /// </summary>
        private IReadOnlyDictionary<int, string> mFaceNames = new Dictionary<int, string>()
        {
            { FRONT_FACE, "Front" },
            { BACK_FACE, "Back" },
            { LEFT_FACE, "Left" },
            { RIGHT_FACE, "Right" },
            { TOP_FACE, "Top" },
            { BOTTOM_FACE, "Bottom" },
        };

        /// <summary>
        /// Prints the details of a single face to the screen.
        /// </summary>
        /// <param name="face"></param>
        private void PrintFace(int face)
        {
            Console.WriteLine($"{mFaceNames[face]} face:");

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    Console.Write($"{mState[face, i * 3 + j]} ");
                }

                Console.WriteLine();
            }
        }
    }
}
