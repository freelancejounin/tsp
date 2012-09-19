using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace TSP
{
    class State
    {
        private ArrayList _routeSF;
        private double _costSF;
        private double[,] _adj;
        private double _lowerBound;

        public State(City[] cities, int f, int d, ArrayList routeSF, double costSF, double[,] adj)
        {
            _routeSF = new ArrayList(routeSF);
            _routeSF.Add(cities[d]);
            _costSF = costSF;
            _costSF += cities[f].costToGetTo(cities[d]);
            CopyArray(adj);
            ToInfinity(f, d);
            DetermineLB();
        }

        public State(double[,] adj)
        {
            _routeSF = new ArrayList();
            _costSF = 0;
            CopyArray(adj);
            DetermineLB();
        }

        public ArrayList routeSF
        {
            get { return _routeSF; }
        }

        public double costSF
        {
            get { return _costSF; }
        }

        public double lowerBound
        {
            get { return _lowerBound; }
        }

        //Used to copy the input adjacency matrix
        private void CopyArray(double[,] iArray)
        {
            _adj = new double[iArray.GetLength(0), iArray.GetLength(1)];
            for (int i = 0; i < iArray.GetLength(0); i++)
                for (int j = 0; j < iArray.GetLength(1); j++)
                    _adj[i, j] = iArray[i, j];
        }

        //Used to update the array to reflect the chosen path
        private void ToInfinity(int f, int d)
        {
            for (int i = 0; i < _adj.GetLength(0); i++)
                _adj[i, d] = 999999;
            for (int j = 0; j < _adj.GetLength(1); j++)
                _adj[f, j] = 999999;
        }

        //Used to quickly determine a lower bound for the state
        private void DetermineLB()
        {
            double lb = _costSF;

            for (int i = 0; i < _adj.GetLength(0); i++)
            {
                double lowestOfRow = 999999;
                
                for (int j = 0; j < _adj.GetLength(1); j++)
                {
                    if (_adj[i, j] < lowestOfRow)
                        lowestOfRow = _adj[i, j];
                }

                if (lowestOfRow != 999999)
                    lb += lowestOfRow;
            }

            _lowerBound = lb;
        }

        //Generate next States from this State
        public ArrayList Expand(City[] cities)
        {
            ArrayList newStates = new ArrayList();

            for (int i = 0; i < _adj.GetLength(0); i++)
            {
                for (int j = 0; j < _adj.GetLength(1); j++)
                {
                    if (_adj[i, j] < 999999)
                        newStates.Add(new State(cities, i, j, _routeSF, _costSF, _adj));
                }
            }

            return newStates;
        }

    }
}
