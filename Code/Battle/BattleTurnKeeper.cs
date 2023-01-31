using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnKeeper
{
    private const int increment = 100;

    //Node contaning refrence to player and its current delay in turn order
    private class Node : IComparable<Node>
    {
        //Player wrapped in node
        private BattleUnit unit;
        //Current turn order
        private int delay;

        //Generate new Node
        public Node(BattleUnit u)
        {
            unit = u;
            delay = -increment/2;
        }

        //Create node with set delay
        public Node(BattleUnit u, int i)
        {
            unit = u;
            delay = i;
        }

        //Create Copy of node
        public Node(Node n)
        {
            unit = n.GetUnit();
            delay = n.GetDelay();
        }

        //Retrive player from node
        public BattleUnit GetUnit() { return unit; }
        //Retrive delay from node
        public int GetDelay() { return delay; }

        //Remove speed from delay
        public void Increment()
        {
            delay += unit.GetSpd();
        }

        //Return palyer and add incremnt to delay
        public BattleUnit Inc()
        {
            delay -= increment;
            return unit;
        }

        //Compare delay of two nodes
        public int CompareTo(Node n)
        {
            if (GetDelay() < n.GetDelay())
                return 1;
            else if (GetDelay() > n.GetDelay())
                return -1;
            else
                return 0;
        }

    }

    private List<Node> node_list;

    private BattleUnit next_turn;
    private BattleUnit[] future_turn;

    //Generate new empty turn order
    public BattleTurnKeeper()
    {
        future_turn = new BattleUnit[10];
        node_list = new List<Node>();
    }
    //Generate new turn order but copy data
    private BattleTurnKeeper(BattleTurnKeeper t)
    {
        node_list = t.CopyList();
    }

    //Add new player into list
    public void AddUnit(BattleUnit u)
    {
        node_list.Add(new Node(u));

        CalculateFuture();
    }
    public void RemoveUnit(BattleUnit u)
    {
        Node temp = null;

        foreach (Node n in node_list)
            if (n.GetUnit().Equals(u))
                temp = n;

        if (temp != null)
            node_list.Remove(temp);

        CalculateFuture();
    }

    //Return next player up
    public BattleUnit PeakNextTurn()
    {
        return next_turn;
    }

    //Return next ten players up
    public BattleUnit[] PeakFutureTurns()
    {
        return future_turn;
    }

    //Generate the next turn
    public void CalculateTurn()
    {
        next_turn = NextTurn();
        CalculateFuture();
    }
    // Generate perdiction of next 10 turns
    public void CalculateFuture()
    {
        BattleTurnKeeper future = new BattleTurnKeeper(this);

        for (int i = 0; i < 10; ++i)
            future_turn[i] = future.NextTurn();
    }

    //Increment list to next turn
    private BattleUnit NextTurn()
    {
        if (node_list.Count == 0)
            return null;

        while (node_list[0].GetDelay() < increment)
        {
            foreach (Node n in node_list)
                n.Increment();

            node_list.Sort();
        }

        return node_list[0].Inc();
    }

    //Copy node_list into a new list
    private List<Node> CopyList()
    {
        List<Node> list = new List<Node>();

        foreach (Node n in node_list)
            list.Add(new Node(n));

        return list;
    }
}