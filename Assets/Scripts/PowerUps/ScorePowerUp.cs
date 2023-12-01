[System.Serializable]

public class ScorePowerUp : PowerUp
{
    public float scoreToAdd;
    public override void Apply(PowerUpManager target)
    {
        Pawn pawn = target.GetComponent<Pawn>();

        if(pawn != null)
        {
            pawn.controller.AddToScore(scoreToAdd);
        }


        /* // Another way to add the score?
        Controller targetController = target.GetComponent<Pawn>().controller;
        targetController.AddToScore(scoreToChange);
        */
    }

    public override void Remove(PowerUpManager target)
    {
        //to implement later
    }

}
