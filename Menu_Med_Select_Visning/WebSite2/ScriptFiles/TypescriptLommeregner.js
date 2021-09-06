var Calculator = /** @class */ (function () {
    function Calculator(number1, number2, operation) {
        this.number1 = number1;
        this.number2 = number2;
        this.operation = operation;
    }
    Calculator.prototype.calculate = function () {
        switch (this.operation) {
            case "+": {
                return this.number1 + this.number2;
            }
            case "-": {
                return this.number1 - this.number2;
            }
            case "*": {
                return this.number1 * this.number2;
            }
            case "/": {
                return this.number1 / this.number2;
            }
            default: {
                console.log("Unsupported operation");
                break;
            }
        }
    };
    return Calculator;
}());
function calculate(number1, number2, operation) {
    var calculator = new Calculator(number1, number2, operation);
    return calculator.calculate();
}
function CalculateResult() {
    try {
        $("#resultinput").val(calculate(Number($("#number1input").val()), Number($("#number2input").val()), $("#operator")[0].innerText).toString());
    }
    catch (_a) {
        alert("Cannot perform operation.");
    }
}
function ClearEverything() {
    $("#resultinput").val("");
    $("#number1input").val("");
    $("#number2input").val("");
}
//# sourceMappingURL=TypescriptLommeregner.js.map