using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    public interface IRubiksCubeSolver
    {
        /// <summary>
        /// Solves the rubiks cube!
        /// </summary>
        /// <param name="cube"></param>
        void SolveCube(RubiksCube cube);
    }

    public class RubiksCubeSolverMostEfficient
    {
    }
}
