using System.Transactions;
using System.Xml.Serialization;

namespace game1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DoMenu();
        }
        static void DoMenu()
        {
            Console.Write("1) Play \n2) Exit \nChoice:");
            int choice = int.Parse(Console.ReadLine());
            if(choice == 1)
            {
                Console.Clear();
                Console.Write("1) Warrior \n2) Mage \n3) Archer \nChoose a class: ");
                int classchoice = int.Parse(Console.ReadLine());
                classMenu(classchoice);
            }
        }
        static void classMenu(int choice)
        {
            string charName = "";
            Console.Clear();
            if(choice == 1)
            {
                Console.Write("Greetings, warrior, what is your name? ");
                charName = Console.ReadLine();
                Warrior player = new Warrior(charName);
                Console.WriteLine($"\nWelcome to the dungeon, {charName} the Warrior.");
                Thread.Sleep(3000);
                Console.Clear();
                doFight(player);
            }
            else if(choice == 2)
            {
                Console.Write("Greetings, mage, what is your name? ");
                charName = Console.ReadLine();
                Mage player = new Mage(charName);
                Console.WriteLine($"\nWelcome to the dungeon, {charName} the Mage.");
                Thread.Sleep(3000);
                Console.Clear();
                doFight(player);
            }
            else if(choice == 3)
            {
                Console.Write("Greetings, archer, what is your name? ");
                charName = Console.ReadLine();
                Archer player = new Archer(charName);
                Console.WriteLine($"\nWelcome to the dungeon, {charName} the Archer.");
                Thread.Sleep(3000);
                Console.Clear();
                doFight(player);
            }
            else
            {
                Console.WriteLine("Invald. Quitting.");
            }
        }
        static void doCounter(Character player, Character villain)
        {
            Random rand = new Random();
            if(rand.Next(1,10) < 3 && villain.Health < 50)
            {
                Console.WriteLine("What! Your hit has been countered by Paras!\nHe takes no damage.");
                villain.Health += player.AttackPower;
            }
        }
        static void doFight(Character player)
        {
            int turn = 1;
            int strongTurn = -3;
            
            Character villain = new Enemy("Dark Lord Paras");
            while(player.Health > 0 && villain.Health > 0)
            {
                bool turncomplete = false;
                while (!turncomplete)
                {
                    Console.Clear();
                    Console.WriteLine($"You are on turn number {turn} \n");
                    Console.WriteLine($"Your health is: {player.Health} \nParas' health is: {villain.Health}\n");
                    Console.Write("Do you want to: \n1)Attack\n2)Strong Attack\n3)Wait\n4)Heal\n\n");
                    int choice = int.Parse(Console.ReadLine());
                    if (choice == 1)
                    {
                        player.Attack(villain);
                        doCounter(player, villain);
                        turncomplete = true;
                    }
                    else if (choice == 2)
                    {
                        if (turn > strongTurn + 3)
                        {
                            player.StrongAttack(villain);
                            strongTurn = turn;
                            turncomplete = true;
                        }
                        else
                        {
                            Console.WriteLine($"You cannot perform a strong attack right now - you have {turn - strongTurn} turns left until you can use this again.");
                            Thread.Sleep(3200);
                        }
                    }
                    else if (choice == 4)
                    {
                        Potion healthPot = new Potion(20);
                        healthPot.Heal(player);
                        turncomplete = true;
                    }
                    else
                    {
                        Console.WriteLine("You decide to wait instead of attacking. Why?");
                        turncomplete = true;
                    }
                }
                Thread.Sleep(2500);
                villain.Attack(player);
                Thread.Sleep(2500);
                turn++;

            }
            Console.WriteLine(player.Health > 0 ? "You defeated the Dark Lord Paras, saving the world!" : "The Dark Lord Paras has taken over the world. Try again.");
        }
    }
    public class Potion
    {
        private int healAmount { get; set; }

        public Potion(int amount)
        {
            healAmount = amount;
        }
        public void Heal(Character target)
        {
            Console.WriteLine($"You drink a potion. Miraculously, you are healed by {healAmount} health points.");
            target.Health += healAmount;
        }
    }
    public abstract class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }

        public Character(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
        }

        public abstract void Attack(Character target);
        public abstract void StrongAttack(Character target);
    }

    public class Warrior : Character
    {
        public Warrior(string name) : base(name,120,15)
        {

        }
        public override void Attack(Character target)
        {
            Console.WriteLine($"{Name} swings a sword at {target.Name}.");
            target.Health -= AttackPower;
        }
        public override void StrongAttack(Character target)
        {
            Console.WriteLine($"{Name} hits {target.Name} like REALLY hard.");
            target.Health -= 2*AttackPower;
        }

    }
    public class Mage : Character
    {
        public Mage(string name) : base(name, 80, 20)
        {

        }
        public override void Attack(Character target)
        {
            Console.WriteLine($"{Name} casts a fireball at {target.Name}");
            target.Health -= AttackPower;
        }
        public override void StrongAttack(Character target)
        {
            Console.WriteLine($"{Name} casts a massive fireball at {target.Name}");
            target.Health -= 2*AttackPower;
        }

    }
    public class Archer : Character
    {
        public Archer(string name) : base(name, 100, 12)
        {

        }
        public override void Attack(Character target)
        {
            Console.WriteLine($"{Name} shoots an arrow at {target.Name}");
            target.Health -= AttackPower;
        }
        public override void StrongAttack(Character target)
        {
            Console.WriteLine($"{Name} fires a volley of arrows over {target.Name}");
            target.Health -= 2*AttackPower;
        }

    }
    public class Enemy : Character
    {
        public Enemy(string name) : base(name, 100, 10) { }

        public override void Attack(Character target)
        {
            Console.WriteLine($"{Name} casts a fireball at {target.Name}");
            target.Health -= AttackPower;
        }
        public override void StrongAttack(Character target) { }
    }
}
