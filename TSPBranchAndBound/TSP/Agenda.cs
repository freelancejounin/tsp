using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace TSP
{
    class Agenda
    {
        //an array of ArrayLists
        private ArrayList[] _states;
        private int count;
        private int _maxCount;
        private int _numPruned;

        public Agenda(int length)
        {
            _states = new ArrayList[length + 1];
            _maxCount = 0;
            _numPruned = 0;
        }

        public Boolean hasNextState()
        {
            if (count > 0)
                return true;

            return false;
        }

        public State nextState
        {
            get
            {
                for (int i = _states.Length - 1; i >= 0; i--)
                {
                    ArrayList tList = _states[i];
                    if (tList != null)
                    {
                        double minBound = 999999;
                        int minPos = -1;
                        for (int j = 0; j < tList.Count; j++)
                        {
                            if (((State)tList[j]).lowerBound < minBound)
                            {
                                minBound = ((State)tList[j]).lowerBound;
                                minPos = j;
                            }
                        }
                        if (minPos != -1)
                        {
                            State temp = (State)tList[minPos];
                            tList.RemoveAt(minPos);
                            count--;
                            return temp;
                        }
                    }
                }
                return null;
            }
        }

        public void Add(State newState)
        {
            int position = newState.routeSF.Count;
            System.Console.WriteLine(position);

            if (_states[position] == null)
                _states[position] = new ArrayList();

            _states[position].Add(newState);

            count++;
            if (count > _maxCount)
                _maxCount = count;
        }

        public int NumPruned
        {
            get { return _numPruned; }
        }

        public int MaxCount
        {
            get { return _maxCount; }
        }

        public void Prune(double bestCost)
        {
            for (int i = _states.Length - 1; i >= 0; i--)
            {
                ArrayList tList = _states[i];
                if (tList != null)
                {
                    for (int j = 0; j < tList.Count; j++)
                    {
                        if (((State)tList[j]).lowerBound >= bestCost)
                        {
                            tList.RemoveAt(j);
                            _numPruned++;
                            count--;
                        }
                    }
                    
                }
            }
        }

    }
}
