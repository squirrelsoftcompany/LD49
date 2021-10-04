using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ModuleData
{
    public bool m_purgePressionPresent = true;
    public bool m_purgeErgolPresent = true;
    public List<Fuel> m_fuels;

    // Difficulty max: 17, min:1
    public const int MaxDiff = 17;
    public const int MinDiff = 1;
    public int Difficulty()
    {
        int difficulty = m_fuels.Sum(x => ErgolDifficulty(x));
        difficulty += m_purgeErgolPresent ? 0 : 2;
        difficulty += m_purgePressionPresent ? 0 : 2;
        return difficulty;
    }

    public static int ErgolDifficulty(Fuel fuel)
    {
        switch (fuel)
        {
            case Fuel.eE1:
            case Fuel.eE2:
            case Fuel.eE3:
                return 1;
            case Fuel.eE4:
            case Fuel.eE5:
            case Fuel.eE6:
                return 2;
            case Fuel.eE7:
                return 3;
        }
        return 0;
    }

    public int MinimalNumberOfConnector()
    {
        if (m_fuels.Any(x => x == Fuel.eE7)) // Full mix fuel
            return 3;
        if (m_fuels.Any(x => (int)x != 1 && (int)x%2 == 1)) // duo-mix fuel
            return 2;
        return 1; // simple fuel
    }

    static Fuel[] actualFuelOrder = { Fuel.eE1, Fuel.eE2, Fuel.eE3, Fuel.eE4, Fuel.eE5, Fuel.eE6, Fuel.eE7 };
    public static ModuleData Generate(int difficultyMin, int difficultyMax)
    {
        if (difficultyMin <= 0 || difficultyMin > difficultyMax)
            return null;

        ModuleData module;
        do
        {
            module = new ModuleData();
            module.m_fuels = new List<Fuel>();

            int nbErgol = difficultyMax >= 9 ? Random.Range(3, 5 + 1) : Random.Range(2, 3 + 1);
            Fuel previousFuel = Fuel.eNull;
            for (int i = 0; i < nbErgol; i++)
            {
                if (module.Difficulty() < difficultyMax)
                {
                    Fuel neoFuel = previousFuel;

                    while (previousFuel == neoFuel || module.Difficulty() + ErgolDifficulty(neoFuel) > difficultyMax)
                    {
                        var rnd = Random.value;
                        if (rnd > 0.5 || difficultyMax < 5) // 50%
                            neoFuel = actualFuelOrder[Random.Range(0, 2+1)];
                        else if (rnd > 0.05) // 95% - 50%: 45%
                            neoFuel = actualFuelOrder[Random.Range(3, 5+1)];
                        else // 5%
                            neoFuel = Fuel.eE7;
                    }

                    module.m_fuels.Add(neoFuel);
                    previousFuel = neoFuel;
                }
            }

            if (difficultyMax - module.Difficulty() > 2 && difficultyMax >= 9)
            {
                module.m_purgeErgolPresent = Random.value > 0.5;
                if (difficultyMax - module.Difficulty() > 2)
                {
                    module.m_purgePressionPresent = Random.value > 0.5;
                }
            }
        } while (module.Difficulty() < difficultyMin || module.Difficulty() > difficultyMax);

        return module;
    }
}

[System.Serializable]
public class RocketData
{
    public bool m_twoDistinctPurgeButton = true; // true: 2 buttons, false: 1 button that does both purge
    public List<ModuleData> m_modules;

    // Difficulty max: 88, min:1
    public const int MaxDiff = 88;
    public const int MinDiff = 1;
    public int Difficulty()
    {
        int difficulty = m_modules.Sum(x => x.Difficulty());
        difficulty += m_twoDistinctPurgeButton ? 0 : 3;
        return difficulty;
    }

    public static RocketData Generate(int difficultyMin, int difficultyMax)
    {
        if (difficultyMin <= 1 || difficultyMin > difficultyMax)
            return null;

        RocketData rocket;
        do
        {
            rocket = new RocketData();
            rocket.m_modules = new List<ModuleData>();

            int moduleCount = ComputeModuleCount(difficultyMin, difficultyMax);
            ComputeModuleMinMax(difficultyMin, difficultyMax, moduleCount, out int moduleDiffMax, out int moduleDiffMin);
            for (int i = 0; i < moduleCount - 1; i++)
            {
                rocket.m_modules.Add(ModuleData.Generate(moduleDiffMin, moduleDiffMax));
            }
            int remainingDifficultyMax = Mathf.Min(difficultyMax - rocket.Difficulty(), 17); // difficulty of a module can't be greater than 17
            int remainingDifficultyMin = Mathf.Clamp(difficultyMin - rocket.Difficulty(), 1, remainingDifficultyMax * 3/4);
            rocket.m_modules.Add(ModuleData.Generate(remainingDifficultyMin, remainingDifficultyMax));
            rocket.m_modules = rocket.m_modules.OrderBy(x => Random.value).ToList(); // shuffle

            if (difficultyMax - rocket.Difficulty() > 2 && difficultyMax >= 44)
            {
                rocket.m_twoDistinctPurgeButton = Random.value > 0.5;
            }
        } while (rocket.Difficulty() < difficultyMin || rocket.Difficulty() > difficultyMax);

        return rocket;
    }

    static int ComputeModuleCount(int difficultyMin, int difficultyMax)
    {
        int moduleCountMin = Mathf.Min((difficultyMax / ModuleData.MaxDiff) + 1, 5);
        int moduleCountMax = Mathf.Min(moduleCountMin + 3, Mathf.Min(difficultyMin / ModuleData.MaxDiff, 5));

        return Random.Range(moduleCountMin, moduleCountMax + 1);
    }

    static void ComputeModuleMinMax(int difficultyMin, int difficultyMax, int moduleCount, out int moduleDiffMax, out int moduleDiffMin)
    {
        moduleDiffMax = Mathf.Clamp( Mathf.RoundToInt(((float)difficultyMax / moduleCount) + difficultyMax / 5), ModuleData.MinDiff, ModuleData.MaxDiff );
        moduleDiffMin = moduleDiffMax / 2;
    }

    public static RocketData GenerateEasyRocket() { return Generate(4,9); }
    public static RocketData GenerateNormalRocket() { return Generate(35, 50); }
    public static RocketData GenerateHardRocket() { return Generate(65, 88); }
}
