using Newtonsoft.Json;
using System.Globalization;
using System.Text;
class Map
{
    Player p;
    Shop sh;
    public void TownScreen()
    {
        Console.Clear();
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
        Console.WriteLine("");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전입장");
        Console.WriteLine("5. 휴식하기");
        Console.WriteLine("0. 저장하기");
        Console.WriteLine("");
        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">>");
        switch (Console.ReadLine())
        {
            case "0":
                Program.SaveObject(p,sh);
                Console.WriteLine("저장되었습니다.");
                Console.ReadKey();
                break;
            case "1":
                p.UserStatus();
                break;
            case "2":
                p.PrintInven();
                break;
            case "3":
                sh.ShopPrint(p);
                break;
            case "4":
                Program.wh = Program.Where.Dungeon;
                return;
            case "5":
                Recovery();
                break;
            default:
                Program.WrongInput();
                break;
        }
    }
    public void Recovery()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {p.gold} G)");
            Console.WriteLine("");
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            switch (Console.ReadLine())
            {
                case "0":
                    return;
                case "1":
                    if (p.hp >= p.maxHp)
                    {
                        Console.WriteLine("회복할 필요 없습니다");
                        Console.ReadKey();
                        break;
                    }
                    if (p.gold >= 500)
                    {
                        Console.WriteLine("휴식을 완료했습니다.");
                        p.gold -= 500;
                        p.hp += 100;
                        if (p.hp > p.maxHp)
                        {
                            p.hp = p.maxHp;
                        }
                        Console.ReadKey();
                    }
                    else if (p.gold < 500)
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                        Console.ReadKey();
                    }
                    break;
                default:
                    Program.WrongInput();
                    break;
            }
        }
    }
    public Map(Player p, Shop sh)
    {
        this.p = p;
        this.sh = sh;
    }
}

class Shop
{
    public class ShopInven
    {
        public EquipItem item;
        public bool canBuy;
        public ShopInven(EquipItem item)
        {
            this.item = item;
            canBuy = true;
        }
    }
    public ShopInven[] inven { get; set; }
    bool BuyMode = false;
    public void ShopPrint(Player P)
    {
        while (true)
        {
            if (BuyMode)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine(P.gold + " G");
                Console.WriteLine("");
                Console.WriteLine("아이템 목록");
                for (int i = 0; i < inven.Length; i++)
                {
                    if (inven[i] == null)
                    {
                        break;
                    }
                    Console.Write("- " + (i + 1) + " ");
                    inven[i].item.ShowItem();
                    if (inven[i].canBuy == true)
                    {
                        Console.WriteLine($"|  {inven[i].item.gold} G");
                    }
                    else
                    {
                        Console.WriteLine($"| 구매완료");
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? input = Console.ReadLine();
                if (Int32.TryParse(input, out int temp) && temp >= 1 && temp <= 9 && inven[temp - 1] != null)
                {
                    bool isFull = P.inven.All(space => space != null);
                    if (!isFull)
                    {
                        for (int i = 0; i < P.inven.Length; i++)
                        {
                            if (P.inven[i] == null)
                            {
                                if (P.gold >= inven[temp - 1].item.gold && inven[temp - 1].canBuy == true)
                                {
                                    P.inven[i] = inven[temp - 1].item;
                                    P.gold -= inven[temp - 1].item.gold;
                                    inven[temp - 1].canBuy = false;
                                    Console.WriteLine("구매를 완료 했습니다");
                                    Console.ReadKey();
                                    break;
                                }
                                else if (P.gold >= inven[temp - 1].item.gold && inven[temp - 1].canBuy == false)
                                {
                                    Console.WriteLine("이미 팔린 아이템 입니다.");
                                    Console.ReadKey();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Gold가 부족합니다.");
                                    Console.ReadKey();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("인벤토리가 가득합니다");
                        Console.ReadKey();
                    }
                    continue;
                }
                else if (input == "0")
                {
                    BuyMode = false;
                    continue;
                }
                else
                {
                    Program.WrongInput();
                    continue;
                }
            }
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(P.gold + " G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < inven.Length; i++)
            {
                if (inven[i] == null)
                {
                    break;
                }
                Console.Write("- ");
                inven[i].item.ShowItem();
                if (inven[i].canBuy == true)
                {
                    Console.WriteLine($"|  {inven[i].item.gold} G");
                }
                else
                {
                    Console.WriteLine($"| 구매완료");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            Console.Write(">>");
            switch (Console.ReadLine())
            {
                case "0":
                    return;
                case "1":
                    BuyMode = true;
                    break;
                case "2":
                    SellItem(P);
                    break;
                default:
                    Program.WrongInput();
                    break;
            }
        }
    }
    void SellItem(Player p)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(p.gold + " G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            p.SortInven();
            for (int i = 0; i < p.inven.Length; i++)
            {
                if (p.inven[i] == null)
                {
                    break;
                }
                Console.Write("- " + (i + 1) + " ");
                p.inven[i].ShowItem();
                Console.WriteLine($"|  {inven[i].item.gold} G");
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            string? input = Console.ReadLine();
            if (Int32.TryParse(input, out int temp) && temp >= 1 && temp <= 9 && p.inven[temp - 1] != null)
            {
                p.gold += (int)(p.inven[temp - 1].gold * 0.85f);
                p.UnWear(p.inven[temp - 1]);
                p.inven[temp - 1] = null;
                continue;
            }
            else if (input == "0")
            {
                return;
            }
            else
            {
                Program.WrongInput();
                continue;
            }
        }
    }

    public Shop()
    {
        inven = new ShopInven[9];
        inven[0] = new ShopInven(new EquipItem("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, EquipItem.Type.Armor, def: 5));
        inven[1] = new ShopInven(new EquipItem("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800, EquipItem.Type.Armor, def: 9));
        inven[2] = new ShopInven(new EquipItem("광선검", "공기중의 에너지로 충전된다", 5000, EquipItem.Type.Weapon, atk: 20));
    }
}

class Player
{
    int _gold; 
    public int lv{ get; set; }
    public int clearCount { get; set; }
    public int needClear { get; set; }
    public int hp { get; set; }
    public int maxHp { get; set; }
    public float atk { get; set; }
    public int def { get; set; }
    public void LvUp()
    {
        clearCount++;
        if (clearCount == needClear)
        {
            lv++;
            atk = atk + 0.5f;
            def++;
            needClear++;
            clearCount = 0;
            return;
        }
    }
    public float totalAtk 
    {
        get
        {
            if (weaponSlot == null)
            {
                return atk;
            }
            return atk + weaponSlot.atk;
        }
    }
    public int totalDef
    {
        get
        {
            return armorSlot == null ? def : def + armorSlot.def;
        }
    }
    bool choiceMode = false;
    public EquipItem? weaponSlot { get; set; }
    public EquipItem? armorSlot { get; set; }
    public int gold
    {
        set { _gold = value; }
        get { return _gold; }
    }
    public EquipItem[] inven { get; set; }
    public void SortInven()
    {
        for (int i = 0; i < inven.Length; i++)
        {
            for (int p = inven.Length - 1; p > 0; p--)
            {
                if (inven[i] == null && inven[p] != null)
                {
                    EquipItem temp = inven[i];
                    inven[i] = inven[p];
                    inven[p] = temp;
                    break;
                }
                if (i == p)
                {
                    return;
                }
            }
        }
    }
    public void UserStatus()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();
            Console.WriteLine("Lv. " + (lv < 10 ? "0" + lv : lv));
            Console.WriteLine("Chad ( 전사 )");
            Console.WriteLine($"공격력 : {atk} {(weaponSlot == null ? "" : $"(+{weaponSlot.atk})")}");
            Console.WriteLine($"방어력 : {def} {(armorSlot == null ? "" : $"(+{armorSlot.def})")}");
            Console.WriteLine("체력 : " + hp + " / " + maxHp);
            Console.WriteLine("Gold : " + _gold + " G");
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            switch (Console.ReadLine())
            {
                case "0":
                    return;
                default:
                    Program.WrongInput();
                    break;
            }
        }
    }
    public void PrintInven()
    {
        SortInven();
        while (true)
       {
            if (choiceMode)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < inven.Length; i++)
                {
                    if (inven[i] == null)
                    {
                        break;
                    }
                    Console.Write("- " + (i + 1) + " ");
                    if (inven[i].name == (armorSlot == null ? "no" : armorSlot.name) || inven[i].name == (weaponSlot == null ? "no" : weaponSlot.name))
                    {
                        Console.Write("[E]");
                    }
                    inven[i].ShowItem();
                    Console.WriteLine("");
                }
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? input = Console.ReadLine();
                if (Int32.TryParse(input, out int temp) && temp >= 1 && temp <= 9 && inven[temp - 1] != null)
                {
                    Wear(inven[temp - 1]);
                    continue;
                }
                else if (input == "0")
                {
                    choiceMode = false;
                    continue;
                }
                else
                {
                    Program.WrongInput();
                    continue;
                }
            }
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < inven.Length; i++)
            {
                if (inven[i] == null)
                {
                    break;
                }
                Console.Write("- ");
                if (inven[i].name == (armorSlot == null ? "no" : armorSlot.name)  || inven[i].name == (weaponSlot == null ? "no" : weaponSlot.name))
                {
                    Console.Write("[E]");
                }
                inven[i].ShowItem();
                Console.WriteLine("");
            }
            Console.WriteLine("");
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            Console.Write(">>");
            switch (Console.ReadLine())
            {
                case "0":
                    return;
                case "1":
                    choiceMode = true;
                    break;
                default:
                    Program.WrongInput();
                    break;
            }
        }
    }
    public void Wear(EquipItem it)
    {
        switch (it.ty)
        {
            case EquipItem.Type.Weapon:
                if (weaponSlot != null)
                {
                    weaponSlot = weaponSlot.name == it.name ? null : it;
                    break;
                }
                weaponSlot = it;
                break;
            case EquipItem.Type.Armor:
                if (armorSlot != null)
                {
                    armorSlot = armorSlot.name == it.name ? null : it;
                    break;
                }
                armorSlot = it;
                break;
        }
    }
    public void UnWear(EquipItem it)
    {
        switch (it.ty)
        {
            case EquipItem.Type.Weapon:
                if (weaponSlot == null)
                {
                    break;
                }
                weaponSlot = weaponSlot.name == it.name ? weaponSlot = null : weaponSlot;
                break;
            case EquipItem.Type.Armor:
                if (armorSlot == null)
                {
                    break;
                }
                armorSlot = armorSlot.name == it.name ? null : armorSlot;
                break;
        }
    }
    public Player()
    {
        lv = 1;
        atk = 10f;
        def = 5;
        maxHp = 100;
        hp = maxHp;
        _gold = 1500;
        needClear = 1;
        inven = new EquipItem[9];
    }
}
class EquipItem
{
    public enum Type
    {
        Weapon, Armor
    }
    public string name { get; set; } 
    public string about { get; set; }
    public int gold { get; set; }
    public float atk { get; set; }
    public int def { get; set; }
    public string? space { get; set; }
    public string? space2 { get; set; }
    public Type ty { get; set; }
    public void ShowItem()
    {
        Console.Write($"{name}{space}   | {(ty == Type.Armor ? "방어력 +" + def : "공격력 +" + atk)} | {about}{space2}");
    }
    public EquipItem(string name, string about, int gold, Type ty, float atk = 0, int def = 0)
    {
        this.name = name;
        this.about = about;
        this.gold = gold;
        this.atk = atk;
        this.def = def;
        this.ty = ty;
        int temp = 0;
        foreach (char item in this.name)
        {
            if (char.GetUnicodeCategory(item) == UnicodeCategory.OtherLetter)
            {
                temp++;
            }
        }
        for (int i = 0; i < 13 - this.name.Length - temp; i++)
        {
            space += " ";
        }
        temp = 0;
        foreach (char item in this.about)
        {
            if (char.GetUnicodeCategory(item) == UnicodeCategory.OtherLetter)
            {
                temp++;
            }
        }
        for (int i = 0; i < 50 - this.about.Length - temp; i++)
        {
            space2 += " ";
        }
    }
}
class Dungeon
{
    Player p;
    public void DungeonPrint()
    {
        while (true)
        {
            if (p.hp <= 0)
            {
                Console.Clear();
                Console.WriteLine("부상을 치유해야 합니다");
                Console.ReadKey();
                Program.wh = Program.Where.SpartaTown;
                return;
            }
            Console.Clear();
            Console.WriteLine("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 쉬운 던전    | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전    | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전  | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            Console.Write(">>");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    Program.wh = Program.Where.SpartaTown;
                    return;
                case "1":
                case "2":
                case "3":
                    DungeonLogic(Int32.Parse(input));
                    break;
                default:
                    Program.WrongInput();
                    break;
            }
        }
    }
    void DungeonLogic(int difficulty)
    {
        int rec = 0;
        int treasure = 0;
        string dif = "";
        switch (difficulty)
        {
            case 1:
                rec = 5;
                treasure = 1000;
                dif = "쉬운";
                break;
            case 2:
                rec = 11;
                treasure = 1700;
                dif = "일반";
                break;
            case 3:
                rec = 17;
                treasure = 2500;
                dif = "어려운";
                break;
        }
        if (p.totalDef < rec)
        {
            if (Program.ran.Next(0,10) <= 3)
            {
                FailDungeon();
                return;
            }
        }
        int tempp;
        if (p.atk % 1 == 0)
        {
            tempp = (int)(Program.ran.Next((int)p.totalAtk, (int)p.totalAtk * 2 + 1) * 0.01f * treasure) + treasure;
        }
        else
        {
            tempp = (int)((Program.ran.Next((int)p.totalAtk, (int)p.totalAtk * 2 + 1) + 0.5f) * 0.01f * treasure) + treasure;
        }
        int a = rec - p.totalDef + Program.ran.Next(25, 36);
        int temp =  p.hp - (a < 0 ? 0 : a);
        q:
        Console.Clear();
        Console.WriteLine("던전 클리어");
        Console.WriteLine("축하합니다!!");
        Console.WriteLine($"{dif} 던전을 클리어 하였습니다");
        Console.WriteLine("");
        Console.WriteLine("[탐험 결과]");
        Console.WriteLine($"체력 {p.hp} -> {temp}");
        Console.WriteLine($"Gold {p.gold} G -> {(tempp + p.gold)} G");
        Console.WriteLine("");
        Console.WriteLine("0. 나가기");
        Console.WriteLine("");
        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">>");
        if (Console.ReadLine() == "0")
        {
            p.hp = temp;
            p.gold += tempp;
            p.LvUp();
            return;
        }
        else
        {
            Program.WrongInput();
            goto q;
        }
    }
    void FailDungeon()
    {
        Console.WriteLine("실패");
        Console.WriteLine($"체력 {p.hp} -> {(int)(p.hp * 0.5f)}");
        p.hp = (int)(p.hp * 0.5f);
        Console.ReadKey();
    }
    public Dungeon(Player p)
    {
        this.p = p;
    }
}
internal class Program
{
    public enum Where
    {
        SpartaTown,Dungeon
    }
    public static StringBuilder sbd = new StringBuilder();
    public static Random ran = new Random();
    public static Where wh = Where.SpartaTown;
    public static void WrongInput()
    {
        Console.WriteLine("잘못된 입력.");
        Console.ReadKey();
    }
    static void GmaeStart()
    {
        Player newP = new Player();
        Shop TownShop = new Shop();
        Map SpartaTown = new Map(newP, TownShop);
        Dungeon dun = new Dungeon(newP);
        while (true)
        {
            switch (wh)
            {
                case Where.SpartaTown:
                    SpartaTown.TownScreen();
                    break;
                case Where.Dungeon:
                    dun.DungeonPrint();
                    break;
            }
        }
    }
    static public void SaveObject(params object[] objects)
    {
        if (!Directory.Exists("D"))
        {
            Directory.CreateDirectory("D");
        }
        for (int i = 0; i < objects.Length; i++)
        {
            string temp = JsonConvert.SerializeObject(objects[i]);
            File.WriteAllText($"D\\consolegame{i}.json",temp);
        }
    }
    static void LoadGame()
    {
        string player = File.ReadAllText("D\\consolegame0.json");
        string shop = File.ReadAllText("D\\consolegame1.json");
        Player? newP = JsonConvert.DeserializeObject<Player>(player);
        Shop? TownShop = JsonConvert.DeserializeObject<Shop>(shop);
        Map SpartaTown = new Map(newP, TownShop);
        Dungeon dun = new Dungeon(newP);
        while (true)
        {
            switch (wh)
            {
                case Where.SpartaTown:
                    SpartaTown.TownScreen();
                    break;
                case Where.Dungeon:
                    dun.DungeonPrint();
                    break;
            }
        }

    }
    static void Main(string[] args)
    {
        Console.WriteLine("1. 처음부터");
        Console.WriteLine("2. 불러오기");
        string? input = Console.ReadLine();
        switch (input)
        {
            case "1":
                GmaeStart();
                break;
            case "2":
                LoadGame();
                break;
            default:
                WrongInput();
                break;
        }
    }
}

