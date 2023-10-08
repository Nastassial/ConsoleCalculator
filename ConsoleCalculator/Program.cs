
using System.Text.RegularExpressions;

string message;

while (true)
{
    Console.Write("Введите выражение: ");
    string? expression = Console.ReadLine();

    if (expression == null || expression == "") break;

    message = executeExpression(expression.Replace(" ", String.Empty));

    if (message != "OK") Console.WriteLine(message);
}

Console.WriteLine("Завершение работы программы...");

static string executeExpression(string expression)
{
    Regex regex = new Regex(@"[^+\-*/0-9]+");

    if (regex.Matches(expression).Count > 0)
        return "В выражении есть недопустимый символ! Возможные символы: 0-9, +, -, *, /";

    int operIndex;
    double fNum, sNum, res;
    string[] operations = { "*", "/", "+", "-" };
    string[] numbers = expression.Split(operations, StringSplitOptions.RemoveEmptyEntries);
    string[] expressionOpers = expression.Split(numbers, StringSplitOptions.RemoveEmptyEntries);

    List<string> mainOpers = new List<string> { "*", "/" };
    List<string> expOperList = new List<string> { };
    List<string> expOperListTmp = expressionOpers.ToList();

    for (int i = 0; i < expressionOpers.Length; i++)
    {
        if (mainOpers.Contains(expressionOpers[i]))
        {
            expOperList.Add(expressionOpers[i]);
            expOperListTmp.Remove(expressionOpers[i]);
        }
    }

    expOperList.AddRange(expOperListTmp);

    foreach (string op in expOperList)
    {
        numbers = expression.Split(operations, StringSplitOptions.RemoveEmptyEntries);
        expressionOpers = expression.Split(numbers, StringSplitOptions.RemoveEmptyEntries);

        operIndex = Array.IndexOf(expressionOpers, op);

        while (operIndex != -1)
        {
            if (!double.TryParse(numbers[operIndex], out fNum) || !double.TryParse(numbers[operIndex + 1], out sNum)) 
                return "Ошибка преобразования числа!";

            res = op switch
            {
                "+" => fNum + sNum,
                "-" => fNum - sNum,
                "*" => fNum * sNum,
                "/" => fNum / sNum,
                _ => 0
            };

            expression = expression.Replace($"{fNum}{op}{sNum}", res.ToString());

            operIndex = Array.IndexOf(expressionOpers, op, operIndex + 1);
        }
    }

    Console.WriteLine($"Результат: {expression}");
    return "OK";
}