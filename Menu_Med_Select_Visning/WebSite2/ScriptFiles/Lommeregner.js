
var operator = null;

var number1input = null;
var number2input = null;

var resultinput = null;

function SetupDOMElements() {
    operator = $("#operator");

    number1input = $("#number1input");
    number2input = $("#number2input");

    resultinput = $("#resultinput");

}

function restoreOp(button) {

    var opDivChildren = button.parentNode.children;

    var i = 0;
    [...$("#currentOp")[0].children].forEach(element => {
        element.value = opDivChildren[i].value
        element.innerText = opDivChildren[i].innerText
        i++;
    })
}

function deleteOp(button) {
    button.parentNode.remove()
}

function addResultRow() {
    
    var cloneOp = $("#currentOp")[0].cloneNode(true);

    var resultList = $("#resultRows")[0];

    let newID = String(resultList.children.length);

    // change IDs and set readonly on clone.
    cloneOp.id = "stackDiv" + newID;
    [...cloneOp.children].forEach(element => {
        element.id = cloneOp.id + element.id;
        element.readOnly = true;
    })

    restoreButton = $("#restoreButton")[0].cloneNode(true);
    restoreButton.id = cloneOp.id + "restoreButton";
    restoreButton.hidden = false;
    cloneOp.appendChild(restoreButton);

    deleteButton = $("#deleteButton")[0].cloneNode(true);
    deleteButton.id = cloneOp.id + "deleteButton";
    deleteButton.hidden = false;
    cloneOp.appendChild(deleteButton);

    var liWrapper = $("#cloneLi")[0].cloneNode(true);
    liWrapper.id = "stackOp" + newID;
    liWrapper.appendChild(cloneOp);

    resultList.appendChild(liWrapper);

    liWrapper.hidden = false;
    
}

function CalculateResult() {

    var operatorString = operator[0].innerText;

    var num1 = parseInt(number1input[0].value);
    var num2 = parseInt(number2input[0].value);

    if ([num1, num2].includes(NaN)) {
        alert("Cannot parse numbers");
        return;
    }

    if (operatorString === '+') {
        resultinput.val(num1 + num2);
    } else if (operatorString === '-') {
        resultinput.val(num1 - num2);
    } else if (operatorString === '*') {
        resultinput.val(num1 * num2);
    } else if (operatorString === '/') {
        resultinput.val(num1 / num2);
    } else {
        alert("Invalid operator!");
        return;
    }

    addResultRow();
}

function ClearEverything() {
    operator[0].innerText = "+";
    number1input.val("0");
    number2input.val("0");
    resultinput.val("0");
}