namespace CardGame.Run
{
    public class RevivePolicy
    {
        private readonly GameRulesSO _rules;

        public RevivePolicy(GameRulesSO rules) {
            _rules = rules;
        }

        public int CostFor(int reviveCount) {
            var costs = _rules.ReviveCosts;
            return costs[System.Math.Min(reviveCount, costs.Count - 1)];
        }
    }
}
