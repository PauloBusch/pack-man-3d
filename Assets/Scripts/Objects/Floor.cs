using Assets.Constants;
using UnityEngine;

public class Floor : WayPointBase
{
    public GameObject Life;
    public GameObject Clover;
    public GameObject Candy;
    public GameObject Ammunition;

    public void CreateCandies()
    {
        foreach (var wayPoint in _wayPoints)
            CreateChild(wayPoint, Candy, Tags.Candy);
    }

    public void CreateRandomLifes(int total)
    {
        for (var count = 1; count <= total; count++)
        {
            var newLife = CreateChild(GetRandom(), Life, Tags.Life);
            newLife.transform.Rotate(-90f, 0f, 0f);
        }
    }

    public void CreateRandomClovers(int total)
    {
        for (var count = 1; count <= total; count++)
            CreateChild(GetRandom(), Clover, Tags.Clover);
    }

    public void CreateRandomAmmunitions(int total)
    {
        for (var count = 1; count <= total; count++)
            CreateChild(GetRandom(), Ammunition, Tags.Ammunition);
    }
}
