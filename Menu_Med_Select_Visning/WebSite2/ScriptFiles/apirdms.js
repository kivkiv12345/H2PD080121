const uri = 'https://cityinfo.buchwaldshave34.dk/api/City';
const countryUri = 'https://cityinfo.buchwaldshave34.dk/api/Country';
const languageUri = 'https://cityinfo.buchwaldshave34.dk/api/Language';
let resposeItems = [];

function getItems() {
    fetch(`${uri}?includeRelations=true&UseLazyLoading=true&UseAutoMapper=true`)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const addDescriptionTextbox = document.getElementById('add-description');
    const addCountrySelect = document.getElementById('add-countrySelect');
    let CountryIdValue = $("#add-countrySelect option:selected").val();

    console.log(CountryIdValue);

    const city = {
        name: addNameTextbox.value.trim(),
        description: addDescriptionTextbox.value.trim(),
        countryid: CountryIdValue,
    };

    //var url = new URL(uri);
    //url.searchParams.append('UserName', username)

    fetch(`${uri}?includeRelations=false&UseLazyLoading=true&UseAutoMapper=true`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(city)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function clearSelect(select) {
    var i, L = select.options.length - 1;
    for (i = L; i >= 0; i--) {
        select.remove(i);
    }
}

function displayEditForm(id) {
    const item = resposeItems.find(item => item.id === id);

    let countrySelect = document.getElementById("edit-countrySelect");
    let languageSelect = document.getElementById('edit-languages');

    clearSelect(countrySelect);
    clearSelect(languageSelect);

    fetch(`${countryUri}?includeRelations=false&UseLazyLoading=true&UseAutoMapper=true`)
        .then(response => response.json())
        .then(data => data.forEach(function (country) {
            let option = document.createElement("option");
            option.value = country.countryID;
            option.text = country.countryName;
            if (country.countryID === item.countryID) {
                option.selected = true;
            }
            countrySelect.appendChild(option);
        }))
        .catch(error => console.error('Unable to get items.', error));

    fetch(`${languageUri}?includeRelations=false&UseLazyLoading=true&UseAutoMapper=true`)
        .then(response => response.json())
        .then(data => data.forEach(function (language) {
            let option = document.createElement("option");
            option.value = language.id;
            option.text = language.languageName;

            // TODO Kevin: This doesn't work!!!!!!!!!!!!!!
            for (i = 0; i < item.cityLanguages.length; i++) {
                if (item.cityLanguages[i].id === language.id) {
                    language.selected = true;
                    break;
                }
            }

            languageSelect.appendChild(option);
        }))
        .catch(error => console.error('Unable to get items.', error));

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-description').value = item.description;
    
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;

    const optionID = $("#add-countrySelect option:selected").val();

    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        description: document.getElementById('edit-description').value.trim(),
        countryID: optionID,
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'item' : 'items';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {

    let newCityCountrySelect = document.getElementById("add-countrySelect");

    clearSelect(newCityCountrySelect);

    fetch(`${countryUri}?includeRelations=false&UseLazyLoading=true&UseAutoMapper=true`)
        .then(response => response.json())
        .then(data => data.forEach(function (country) {
            let option = document.createElement("option");
            option.value = country.countryID;
            option.text = country.countryName;
            newCityCountrySelect.appendChild(option);
        }))
        .catch(error => console.error('Unable to get items.', error));

    const tBody = document.getElementById('responseTable');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td2 = tr.insertCell(0);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.description);
        td3.appendChild(textNode2);

        let td4 = tr.insertCell(2);
        let textNode3 = document.createTextNode(item.country.countryName);
        td4.appendChild(textNode3);

        let td5 = tr.insertCell(3);
        let text = "";
        item.cityLanguages.forEach(function (lang) { text += lang.languageName + ", " })
        let textNode4 = document.createTextNode(text);
        td5.appendChild(textNode4);

        let td6 = tr.insertCell(4);
        td6.appendChild(editButton);

        let td7 = tr.insertCell(5);
        td7.appendChild(deleteButton);
    });

    resposeItems = data;
}