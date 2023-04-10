using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LAB2_RiPOD
{
    public class ASAP
    {
        private int _countOperations = 0;
        private int _countTypes = 0;
        private int _maxProcessors = 0;

        public int CountTypes
        {
            get => _countTypes;
            set => _countTypes = value;
        }

        public int MaxProcessorsCount
        {
            get => _maxProcessors;
            set => _maxProcessors = value;
        }

        public int CountOperations
        {
            get => _countOperations;
            set => _countOperations = value;
        }

        // p. o. x.

        public int[][] arrayTypes;
        public int[][] arrayH;
        public List<List<int>> List_chains = new List<List<int>>();
        private List<List<bool>> List_chains_ready_for_step = new List<List<bool>>();
        public List<List<int>> steps = new List<List<int>>();
        public List<List<int>> countOperationsForType = new List<List<int>>();
        public List<int> countProcessorsByTypes = new List<int>();

        public ASAP()
        {
            _maxProcessors = 0;
        }

        public void Clear()
        {
            _countOperations = 0;
            _countTypes = 0;
            arrayTypes = new int[][] { };
            arrayH = new int[][] { };
            List_chains = new List<List<int>>();
            List_chains_ready_for_step = new List<List<bool>>();
            steps = new List<List<int>>();
            countOperationsForType = new List<List<int>>();
            countProcessorsByTypes = new List<int>();
            _maxProcessors = 0;
        }

        private string FindValue(string s)
        {
            int index = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ':')
                {
                    index = i + 2;
                }
            }

            string value = "";
            s += '\0';
            while (s[index] != '\0')
            {
                value += s[index];
                index++;
            }

            return value;

        }

        private int[] GetArray(string s)
        {

            string[] nums = s.Split(' ');
            int[] arr = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                arr[i] = int.Parse(nums[i]);
            }

            return arr;
        }

        public void ReadFile(string filePath)
        {
            // получение данных из файла

            StreamReader file = new StreamReader(filePath);
            string buff = file.ReadLine();

            buff = FindValue(buff);
            _countOperations = Convert.ToInt32(buff);

            buff = "";
            buff = file.ReadLine();
            buff = FindValue(buff);
            _countTypes = Convert.ToInt32(buff);

            arrayTypes = new int[_countTypes][];

            for (int i = 0; i < _countTypes; i++)
            {
                buff = "";
                buff = file.ReadLine();
                buff = FindValue(buff);
                arrayTypes[i] = GetArray(buff);
            }

            buff = "";
            buff = file.ReadLine();

            arrayH = new int[_countOperations][];

            for (int i = 0; i < _countOperations; i++)
            {
                buff = "";
                buff = file.ReadLine();
                buff = FindValue(buff);
                arrayH[i] = GetArray(buff);
            }

            file.Close();
        }

        public void Planning()
        {
            GetChains();
            List_chains_ready_for_step = CreateCloneChainsReady();
            WriteSteps();
            CountTypesOnSteps();
            CountProcessorsEveryType();
            _maxProcessors = MaxProcessorsSum(countProcessorsByTypes);

        }

        private void CountProcessorsEveryType()
        {
            for (int i = 0; i < _countTypes; i++)
            {
                int max = 0;

                for (int k = 0; k < countOperationsForType.Count; k++)
                {
                    if (max < countOperationsForType[k][i])
                    {
                        max = countOperationsForType[i][k];
                    }
                }

                countProcessorsByTypes.Add(max);
            }
        }

        private int MaxProcessorsSum(List<int> L)
        {
            int sum = 0;

            for (int i = 0; i < L.Count; i++)
            {
                sum += L[i];
            }

            return sum;
        }

        private void CountTypesOnSteps()
        {
            for (int i = 0; i < steps.Count; i++)
            {
                countOperationsForType.Add(new List<int>());

                // добавить в список количества операций по типам нули
                for (int k = 0; k < _countTypes; k++)
                {
                    countOperationsForType[i].Add(0);

                    for (int t = 0; t < steps[i].Count; t++)
                    {
                        if (arrayTypes[k].Contains(steps[i][t] + 1))
                        {
                            countOperationsForType[i][k]++;
                        }
                    }
                }
            }

        }

        private void WriteSteps()
        {
            // step one

            steps.Add(new List<int>());
            int index_step = 0;
            // добавление на первый шаг все операции на началах чепочек
            for (int i = 0; i < List_chains.Count; i++)
            {
                steps[index_step].Add(List_chains[i][0]);
            }


            bool exist_nonplanOperation = false;
            // 2 и более шаги
            while (true)
            {
                exist_nonplanOperation = false;
                // проверка на наличие нераспланированных шагов
                for (int i = 0; i < List_chains_ready_for_step.Count; i++)
                {
                    if (List_chains_ready_for_step[i].Contains(false))
                    {
                        exist_nonplanOperation = true;
                        break;
                    }
                }
                // если нераспланированных шагов не осталось, то выйти
                if (!exist_nonplanOperation)
                {
                    break;
                }

                // добавление нового шага
                steps.Add(new List<int>());
                index_step++;
                int index_false = 0;

                for (int i = 0; i < List_chains_ready_for_step.Count; i++)
                {
                    // поиск индекса в цепи, где значение = false
                    index_false = FindIndex(List_chains_ready_for_step[i]);

                    // если есть значение индекса и предшественники операции по индексу
                    // уже распланированы
                    if (index_false != -1 && CheckPredecessors(List_chains[i][index_false]))
                    {
                        // против дублирования обработки одной и той же операции
                        if (!steps[index_step].Contains(List_chains[i][index_false]))
                        {
                            steps[index_step].Add(List_chains[i][index_false]);
                        }

                    }
                }

                // изменение на true всех операций, которые распределены на текущем шаге
                for (int i = 0; i < List_chains_ready_for_step.Count; i++)
                {
                    index_false = FindIndex(List_chains_ready_for_step[i]);
                    if (index_false != -1)
                    {
                        List_chains_ready_for_step[i][index_false] = true;
                    }
                }
            }
        }

        private bool CheckPredecessors(int operation)
        {
            //проверить всех предшественников, все ли они распранированы
            List<bool> predesessors = new List<bool>();
            bool check = false;
            for (int i = 0; i < List_chains.Count; i++)
            {
                int index = FindIndex(List_chains[i], operation);
                index--;
                if (index != -2)
                {
                    predesessors.Add(List_chains_ready_for_step[i][index]);
                }
            }
            // проверить в списке на наличие хоть одного нераспланированного
            check = !predesessors.Contains(false);
            return check;
        }

        private int FindIndex(List<bool> L)
        {
            int index = -1;
            for (int i = 0; i < L.Count; i++)
            {
                if (L[i] == false)
                {
                    index = i;
                    return index;
                }
            }

            return index;
        }

        private int FindIndex(List<int> L, int operation)
        {
            int index = -1;
            for (int i = 0; i < L.Count; i++)
            {
                if (L[i] == operation)
                {
                    index = i;
                    return index;
                }
            }

            return index;
        }

        private List<List<bool>> CreateCloneChainsReady()
        {
            List<List<bool>> ready = new List<List<bool>>();
            for (int i = 0; i < List_chains.Count; i++)
            {
                ready.Add(new List<bool>());

                for (int k = 0; k < List_chains[i].Count; k++)
                {
                    if (k == 0)
                    {
                        ready[i].Add(true);
                    }
                    else
                    {
                        ready[i].Add(false);
                    }
                }
            }

            return ready;
        }
        private void GetChains()
        {
            // построить цепи зависимостей
            List<int> List_use = new List<int>();

            List<int> Chain = new List<int>();
            int operation = 0;

            while (List_use.Count < _countOperations)
            {
                operation = 0;
                while (List_use.Contains(operation))
                {
                    operation++;
                }

                Chain.Clear();
                Chain.Add(operation);
                List_use.Add(operation);
                while (true)
                {
                    // найти в строке таблицы смежности следующее направление
                    bool index = false;
                    for (int i = 0; i < arrayH[operation].Length; i++)
                    {
                        if (arrayH[operation][i] == 1)
                        {
                            index = true;
                            operation = i;
                            break;
                        }
                    }

                    if (index)
                    {
                        Chain.Add(operation);
                        if (!List_use.Contains(operation))
                        {
                            List_use.Add(operation);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                List_chains.Add(new List<int>(Chain));
            }

        }

        public int FindTypes(int num)
        {
            int type = 0;
            for (type = 0; type < _countTypes; type++)
            {
                if (arrayTypes[type].Contains(num))
                {
                    return type;
                }
            }
            return -1;
        }

    }
}

