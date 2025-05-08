using System.Numerics;
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
            //Console.Write("1) Play \n2) Exit \nChoice:");
            //int choice = int.Parse(Console.ReadLine());
            //if(choice == 1)
            //{
            Console.Clear();
            Console.Write("1) Warrior \n2) Mage \n3) Archer \nChoose a class: ");
            int classchoice = int.Parse(Console.ReadLine());
            classMenu(classchoice);
            //}
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
        static void doFight(Character player)
        {
            int turn = 1;
            int strongTurn = -3;
            
            Enemy villain = new Enemy("Dark Lord Paras");
            while(player.Health > 0 && villain.Health > 0)
            {
                bool turncomplete = false;
                while (!turncomplete)
                {
                    Console.Clear();
                    Console.WriteLine($"You are on turn number {turn} \n");
                    Console.WriteLine($"Your health is: {player.Health} \nParas' health is: {villain.Health}\n");
                    Console.Write("Do you want to: \n1)Attack\n2)Strong Attack\n3)Wait\n4)View Potions\n\n");
                    int choice = int.Parse(Console.ReadLine());
                    if (choice == 1)
                    {
                        if (villain.CounterAttack(player, villain))
                        {
                            Console.WriteLine("What! Paras has expertly dodged your attack. He takes no damage.");
                        }
                        else
                        {
                            player.Attack(villain);
                        }
                        turncomplete = true;
                    }
                    else if (choice == 2)
                    {
                        if (turn > strongTurn + 3)
                        {
                            if(villain.CounterAttack(player,villain))
                            {
                                Console.WriteLine("What! Paras has expertly dodged your attack. He takes no damage.");
                            }
                            else
                            {
                                player.StrongAttack(villain);
                                strongTurn = turn;
                            }
                            turncomplete = true;
                        }
                        else
                        {
                            Console.WriteLine($"You cannot perform a strong attack right now - you have {3 - (turn-strongTurn)} turns left until you can use this again.");
                            Thread.Sleep(3200);
                        }
                    }
                    else if (choice == 4)
                    {
                        PotionMenu(player, turn);
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
        static void PotionMenu(Character target, int turn)
        {
            Potion potObj = new Potion();
            int choice = 0;
            Console.WriteLine("Potion menu:");
            if (target is Mage)
            {
                Console.WriteLine("1) Lesser Potion of Healing\n2) Lesser Potion of Magika");
            }
            else if(target is Warrior)
            {
                Console.WriteLine("1) Lesser Potion of Healing\n2) Lesser Potion of Utmost Strength");
            }
            else if(target is Archer)
            {
                Console.WriteLine("1) Lesser Potion of Healing\n2) Lesser Potion of Bewildering Accuracy");
            }
            Console.Write("Which potion to use? ");
            choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                potObj.Heal(target, 20);
            }
            else if(choice == 2 && target is Mage)
            {
                potObj.Magika(target, 10);
            }
            else if(choice == 2)
            {
                potObj.Strength(target, turn, 2);
            }
        }
    }
    public class Potion
    {
        public Potion()
        {
        }
        public void Heal(Character target, int healAmount)
        {
            Console.WriteLine($"You drink a potion. Miraculously, you are healed by {healAmount} health points.");
            target.Health += healAmount;
        }
        public void Magika(Character target, int healAmount)
        {
            Console.WriteLine($"You drink a potion. Miraculously, your magika is increased by {healAmount} points.");
            target.Magika += healAmount;
        }
        public void Strength(Character target, int turn, int duration)
        {
            Console.WriteLine("You drink a potion. Miraculously, you feel stronger.");
            while(turn <= duration)
            {
                target.AttackPower = 2 * target.AttackPower;
            }
        }
    }
    public abstract class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Magika { get; set; }

        public Character(string name, int health, int attackPower,int magika)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
            Magika = magika;
        }

        public abstract void Attack(Character target);
        public abstract void StrongAttack(Character target);
    }
    public class Warrior : Character
    {
        public Warrior(string name) : base(name,120,15,0)
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
        public int magika = 50;
        public Mage(string name) : base(name, 80, 20, 50)
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
        public Archer(string name) : base(name, 100, 12,0)
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
        Random rnd = new Random();
        public Enemy(string name) : base(name, 100, 10, 0) { }

        public override void Attack(Character target)
        {
            Console.WriteLine($"{Name} casts a fireball at {target.Name}");
            target.Health -= AttackPower;
        }
        public override void StrongAttack(Character target) { }
        public bool CounterAttack(Character target, Character villain)
        {
            if((rnd.Next(1,10) < 4) && villain.Health < 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
