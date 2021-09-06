class Calculator {
    number1: number;
    number2: number;
    operation: string;

    constructor(number1: number, number2: number, operation: string) {
        this.number1 = number1;
        this.number2 = number2;
        this.operation = operation;
    }

    calculate() {
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
    }
}

function calculate(number1: number, number2: number, operation: string): number {
    let calculator: Calculator = new Calculator(number1, number2, operation);
    return calculator.calculate();
}

function CalculateResult() {
    try {
        $("#resultinput").val(calculate(
            Number($("#number1input").val()),
            Number($("#number2input").val()),
            $("#operator")[0].innerText
        ).toString());
    } catch {
        alert("Cannot perform operation.")
    }
}

function ClearEverything() {
    $("#resultinput").val("");
    $("#number1input").val("");
    $("#number2input").val("");
}