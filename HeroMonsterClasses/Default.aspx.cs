using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HeroMonsterClasses
{
    
    public partial class Default : System.Web.UI.Page
    {
        
        private void displayStats(Actor opponent1, Actor opponent2)
        {

            
            if (opponent1.Health < 0 && opponent2.Health < 0)
            {
                resultLabel.Text += "<br/><hr/><strong>Both combatants have died!</strong>";
                return;
            }

            Actor winner = opponent1.Health > 0 ? opponent1 : opponent2;
            Actor loser = opponent1.Health <= 0 ? opponent1 : opponent2;


            resultLabel.Text += $"<br/><hr/><strong>{winner.Name} ({winner.Health}) vanquishes {loser.Name} ({loser.Health}) in battle!</strong>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Dice dice = new Dice();

            bool heroSneakAttack = dice.Roll() > 2 ? true : false;
            bool monsterSneakAttack = dice.Roll() > 4 ? true : false;

            Actor hero = new Actor("Ludenmar", 40, 3, 10, heroSneakAttack);
            Actor monster = new Actor("Orc", 60, 0, 8, monsterSneakAttack);

            int round = 1;

            StringBuilder sb = new StringBuilder();

            sb.Append($"{hero.GetStats()}<br/>{monster.GetStats()}<br/><br/>");

            while (hero.Health >= 0 && monster.Health >= 0)
            {
                sb.Append($"<strong>Round {round}</strong><hr/><br/>");

                if (round == 1)
                {
                    if (hero.SneakAttack)
                    {
                        monster.Defend(hero.Attack(dice));
                        sb.Append($"<strong>Sneak attack by {hero.Name}!</strong><br/>");
                        sb.Append($"{hero.GetStats()}<br/>{monster.GetStats()}<br/><br/>");
                    }
                    if (monster.SneakAttack)
                    {
                        hero.Defend(monster.Attack(dice));
                        sb.Append($"<strong>Sneak attack by {monster.Name}!</strong><br/>");
                        sb.Append($"{hero.GetStats()}<br/>{monster.GetStats()}<br/><br/>");
                    }
                }

                hero.Defend(monster.Attack(dice));
                sb.Append($"{hero.GetStats()}<br/>{monster.GetStats()}<br/><br/>");
                monster.Defend(hero.Attack(dice));
                sb.Append($"{hero.GetStats()}<br/>{monster.GetStats()}<br/><br/>");
                round++;
            }
            resultLabel.Text = sb.ToString();
            displayStats(hero, monster);
        }



        private class Actor
        {
            private Random random = new Random();

            public string Name { get; set; }
            public int Health { get; set; }
            public int Defense { get; set; }
            public int Damage { get; set; }
            public bool SneakAttack { get; set;  }

            public Actor(string name, int health, int defense, int damage, bool sneakAttack )
            {
                Name = name;
                Health = health;
                Defense = defense;
                Damage = damage;
                SneakAttack = sneakAttack;
            }

            public int Attack(Dice dice)
            {
                dice.Sides = Damage;
                return dice.Roll();
            }

            public void Defend(int damage)
            {
                int damageTaken = damage - Defense < 0 ? 0 : damage - Defense;
                Health -= damageTaken;
            }

            public string GetStats()
            {
                return $"{Name} - Health: {Health} - Defense: {Defense} - Damage: {Damage}";
            }

        }

        public  class Dice
        {
            private Random random = new Random();

            public int Sides { get; set; }

            public Dice(int sides = 6)
            {
                Sides = sides;
            }

            public int Roll()
            {
                return random.Next(1, Sides);
            }
        }
    }


}