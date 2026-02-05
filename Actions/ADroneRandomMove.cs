using FSPRO;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evil_Riggs.Actions
{
    public class ADroneRandomMove : CardAction
    {
        public int dir;

        //public bool isRandom;

        public bool playerDidIt = true;

        public override void Begin(G g, State s, Combat c)
        {
            timer *= 0.5;
            if (s.rngActions.Next() < 0.5)
            {
                dir *= -1;
            }

            if (dir == 0)
            {
                return;
            }

            foreach (KeyValuePair<int, StuffBase> item in c.stuff.OrderBy<KeyValuePair<int, StuffBase>, int>((KeyValuePair<int, StuffBase> x) => (dir >= 0) ? (-x.Key) : x.Key).ToList())
            {
                DoMoveSingleDrone(s, c, item.Key, dir, playerDidIt);
            }

            Audio.Play(Event.Move);
        }

        public static void DoMoveSingleDrone(State s, Combat c, int x, int dir, bool playerDidIt)
        {
            if (!c.stuff.TryGetValue(x, out StuffBase? value) || value.Immovable())
            {
                return;
            }

            c.stuff.Remove(x);
            int num = dir;
            bool flag = true;
            while (num != 0 && flag)
            {
                int num2 = Math.Sign(num);
                num -= num2;
                value.x += num2;
                if (value.x < c.leftWall || value.x >= c.rightWall)
                {
                    c.stuff[value.x] = value;
                    Audio.Play(Event.Hits_HitDrone);
                    c.DestroyDroneAt(s, value.x, playerDidIt);
                    flag = false;
                }

                if (!c.stuff.ContainsKey(value.x))
                {
                    continue;
                }

                bool flag2 = false;
                if (!value.Invincible())
                {
                    StuffBase value2 = c.stuff[value.x];
                    c.stuff[value.x] = value;
                    Audio.Play(Event.Hits_HitDrone);
                    flag2 = true;
                    c.DestroyDroneAt(s, value.x, playerDidIt);
                    c.stuff[value.x] = value2;
                    flag = false;
                }

                if (!c.stuff[value.x].Invincible() || value.Invincible())
                {
                    c.DestroyDroneAt(s, value.x, playerDidIt);
                    if (!flag2)
                    {
                        Audio.Play(Event.Hits_HitDrone);
                    }
                }
            }

            if (flag)
            {
                c.stuff[value.x] = value;
            }
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            if (s.route is Combat combat)
            {
                foreach (StuffBase value in combat.stuff.Values)
                {
                    if (!value.Immovable())
                    {
                        value.hilight = 2;
                    }
                }
            }

            int num = Math.Abs(dir);

            return
            [
                new GlossaryTooltip(key: "ADroneRandomMove")
                {
                    Icon = StableSpr.icons_droneMoveRandom,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "ADroneRandomMove", "title"]),
                    TitleColor = Colors.card,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "ADroneRandomMove", "desc"], new { cnt = num })
                },
            ];
            //list = new List<Tooltip>();
            //list.Add((dir > 0) ? new TTGlossary("action.droneMoveRight", num) : new TTGlossary("action.droneMoveLeft", num));
            //return list;
        }

        public override Icon? GetIcon(State s)
        {
            //return new Icon(isRandom ? StableSpr.icons_droneMoveRandom : ((dir > 0) ? StableSpr.icons_droneMoveRight : StableSpr.icons_droneMoveLeft), Math.Abs(dir), Colors.drone);
            return new(StableSpr.icons_droneMoveRandom, Math.Abs(dir), Colors.drone);
        }
    }
}
