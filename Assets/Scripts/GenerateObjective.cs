using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum objType { GREATER = 0, LESSER }

public class GenerateObjective : MonoBehaviour
{
    private bool debug = false;
    private int fnum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (debug) if (++fnum % 64 == 0) debugObjective();
    }

    public bool evaluator(List<int> nums, List<char> ops, (objType type, int target) objToken)
    {
        var expression = nums[0].ToString();
        nums.RemoveAt(0);
        while (nums.Count > 0)
        {
            expression += " " + ops[0] + " " + nums[0];
            ops.RemoveAt(0);
            nums.RemoveAt(0);
        }
        var actual = Eval(expression);
        return objToken.type == objType.GREATER ? actual > objToken.target : actual < objToken.target;
    }

    void debugObjective()
    {
        var nums = new List<int>();
        for (int i = 0; i < 5; ++i) nums.Add(Random.Range(1, 11));
        var ops = new List<char>();
        for (int i = 0; i < 4; ++i)
        {
            var op = Random.Range(0, 4);
            if (op == 0) ops.Add('*');
            else if (op == 1) ops.Add('/');
            else if (op == 2) ops.Add('+');
            else ops.Add('-');
        }
        generateObjective(nums, ops);
        return;
    }

    public (string objStr, (objType type, int target) objToken) generateObjective(List<int> nums, List<char> ops)
    {
        nums.Sort();
        ops.Sort();
        var samples = new List<int>();
        for (int i = 0; i < 100; ++i) samples.Add(Sample(nums.Count, nums, ops));
        samples = samples.Distinct().ToList();
        samples.Sort();
        var type = (objType)Random.Range(0, 2);
        int target;
        string objStr;
        if (type == objType.GREATER)
        {
            target = samples[samples.Count * Random.Range(90, 100) / 100];
            target *= target > 0 ? Random.Range(90, 101) : Random.Range(100, 111);
            target = target / 100 - 1;
            objStr = "a number greater than " + target.ToString();
        }
        else
        {
            target = samples[samples.Count * Random.Range(0, 10) / 100];
            target *= target > 0 ? Random.Range(100, 111) : Random.Range(90, 101);
            target = target / 100 + 1;
            objStr = "a number less than " + target.ToString();
        }
        //Test code. If the objective cannot be verified, output debug info and alter return value.
        var isImpossible = !(type == objType.GREATER ? samples.Last() > target : samples.First() < target);
        if (debug || isImpossible)
        {
            Debug.Log("Values in hand: " + string.Join(", ", nums));
            Debug.Log("Operators in hand: " + string.Join(", ", ops));
            Debug.Log("Sampled outputs: " + string.Join(", ", samples));
            Debug.Log("Objective chosen: " + objStr);
            Debug.Log("Objective token: " + (type, target).ToString());
            Debug.Log(isImpossible ? "Objective impossible!" : "Objective verified.");
            if (isImpossible) objStr = "error contact developer with logs";
        }

        return (objStr, (type, target));
    }

    int Sample(int nnums, List<int> nums, List<char> ops)
    {
        var nums_index = new List<int>();
        var ops_index = new List<int>();
        while (nums_index.Count < nnums)
        {
            int r = Random.Range(0, nums.Count);
            bool match = false;
            foreach (var n in nums_index) if (n == r) match = true;
            if (!match) nums_index.Add(r);
        }
        while (ops_index.Count < nnums - 1)
        {
            int r = Random.Range(0, ops.Count);
            bool match = false;
            foreach (var n in ops_index) if (n == r) match = true;
            if (!match) ops_index.Add(r);
        }
        var expression = nums[nums_index[0]].ToString();
        nums_index.RemoveAt(0);
        while (nums_index.Count > 0)
        {
            expression += " " + ops[ops_index[0]] + " " + nums[nums_index[0]];
            ops_index.RemoveAt(0);
            nums_index.RemoveAt(0);
        }
        var rval = Eval(expression);
        return rval;
    }

    int Eval(string expression)
    {
        var table = new System.Data.DataTable();
        return System.Convert.ToInt32(table.Compute(expression, string.Empty));
    }
}
