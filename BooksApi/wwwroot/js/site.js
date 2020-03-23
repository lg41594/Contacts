const url = "api/Contacts";
const span = `<span onclick="this.parentElement.style.display = 'none'" class="w3-button w3-transparent w3-display-right">&times;</span>`;

let Contacts = [];

function getItems() {
    fetch(url).then(response => response.json()).then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items', error));
}

function _hideNumbers() {
    const tBody = document.getElementById("Numbers");
    tBody.innerHTML = " ";
    $('#tNumbers').hide();
}

function _showNumbers(data) {

    $("#tNumbers tbody tr").remove();

    const tBody = document.getElementById("Numbers");
    tBody.innerHTML = " ";

    $("#tNumbers").show();

    data.forEach(item => {
            let tr = tBody.insertRow();

            let td1 = tr.insertCell(0);
            let txtNumber = document.createTextNode(item.number);
            td1.appendChild(txtNumber);
        }
    );
}

function formatDate(dateOfBirth) {
    var date = new Date(dateOfBirth);
    var formatted = date.getFullYear() +
        "-" +
        ("0" + (date.getMonth() + 1)).slice(-2) +
        "-" +
        ("0" + date.getDate()).slice(-2);
    return formatted;
}

function displayEditForm(id) {
    const item = Contacts.find(item => item.contactId === id);

    document.getElementById("edit-id").value = id;
    document.getElementById("edit-name").value = item.name;
    document.getElementById("edit-address").value = item.address;
    document.getElementById("edit-dateOfBirth").value = formatDate(item.dateOfBirth);

    for (var index = 0; index < item.numbers.length; ++index) {
        var li = createListItem(item.numbers[index].number, item.numbers[index].phoneNumberId);
        $("ul.listEdit").append(li);
    }

    document.getElementById("editForm").style.display = "block";

}

function closeInput() {
    document.getElementById("edit-id").value = "";
    document.getElementById("edit-name").value = "";
    document.getElementById("edit-address").value = "";
    document.getElementById("edit-dateOfBirth").value = "";
    document.getElementById("fNewPhoneEdit").value = "";

    $("ul.listEdit>li").remove();

    document.getElementById("editForm").style.display = "none";
}


function updateItem() {
    const itemId = document.getElementById("edit-id").value;
    const editNameTxt = document.getElementById("edit-name");
    const editAddressTxt = document.getElementById("edit-address");
    const editDateOfBirthTxt = document.getElementById("edit-dateOfBirth");
    
    const numbers = $('ul.listEdit>li').toArray();

    var jsonNumbers = [];

    for (var index = 0; index < numbers.length; ++index) {
        var txtPhoneNumberId = numbers[index].children[0].value;
        var txt = numbers[index].textContent;
        txt = txt.replace("x", "");

        var obj = { number: txt, phoneNumberId: txtPhoneNumberId};
        jsonNumbers.push(obj);
    }

    const item = {
        contactId: itemId,
        name: editNameTxt.value.trim(),
        address: editAddressTxt.value.trim(),
        dateOfBirth: new Date(editDateOfBirthTxt.value.trim()),
        numbers: jsonNumbers
    };

    fetch(`${url}/${itemId}`,
        {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        }).then(() => getItems()).catch(error => console.error('Unable to update item', error));

    closeInput();
    return false;
}

function deleteItem(id) {
    fetch(`${url}/${id}`,
        {
             method: 'DELETE'
        }).then(() => getItems()).catch(error => console.error('Unable to delete item', error));
}

function _emptyAddForm() {
    document.getElementById("add-name").value = "";
    document.getElementById("add-address").value = "";
    document.getElementById("add-dateOfBirth").value = "";

    $("ul.list>li").remove();
}

function _displayItems(data) {
    _hideNumbers();
    _emptyAddForm();
    closeInput();

    const button = document.createElement("button");

    const tBody = document.getElementById("Contacts");
    tBody.innerHTML = " ";

    data.forEach(item => {
            let tr = tBody.insertRow();

            let td1 = tr.insertCell(0);
            let txtName = document.createTextNode(item.name);
            td1.appendChild(txtName);

            let td2 = tr.insertCell(1);
            let txtAddress = document.createTextNode(item.address);
            td2.appendChild(txtAddress);


            var date = new Date(item.dateOfBirth);
            var formatted = date.getFullYear() +
                "-" +
                ("0" + (date.getMonth() + 1)).slice(-2) +
                "-" +
                ("0" + date.getDate()).slice(-2);

            let td3 = tr.insertCell(2);
            let txtDateOfBirth = document.createTextNode(formatted);
            td3.appendChild(txtDateOfBirth);

            let editButton = button.cloneNode(false);
            editButton.innerText = "Edit";
            editButton.setAttribute('onclick', `displayEditForm("${item.contactId}")`);
            let td4 = tr.insertCell(3);
            td4.appendChild(editButton);

            let deleteButton = button.cloneNode(false);
            deleteButton.innerText = "Delete";
            deleteButton.setAttribute('onclick', `deleteItem("${item.contactId}")`);
            let td5 = tr.insertCell(4);
            td5.appendChild(deleteButton);
        }
    );

    Contacts = data;

    genTables();

    $("#tContacts2 tr").click(function () {
        $(this).addClass("selected").siblings().removeClass("selected");

        var name = $(this).find('td:eq(0)').text();
        var address = $(this).find('td:eq(1)').text();

        const item = Contacts.find(item => item.name === name && item.address === address);
        _showNumbers(item.numbers);
    });

    document.getElementById("addPhoneNumber").onclick = function () {
        var text = document.getElementById("fNewPhone").value;

        var li = createListItem(text, "");
        $("ul.list").append(li);

        document.getElementById("fNewPhone").value = "";
    }

    document.getElementById("addPhoneNumberEdit").onclick = function () {
        var text = document.getElementById("fNewPhoneEdit").value;

        var li = createListItem(text, "");
        $("ul.listEdit").append(li);

        document.getElementById("fNewPhoneEdit").value = "";
    }

}

function createListItem(text, id) {
    var node = document.createElement("LI");
    node.setAttribute('class', 'w3-display-container');

    var span = document.createElement("SPAN");
    span.setAttribute('class', 'closeSpan w3-button w3-transparent w3-display-right');
    span.setAttribute('onclick', "this.parentElement.style.display = 'none'");
    span.innerText = "x";

    var textNode = document.createTextNode(text);
    //textNode.setAttribute('class', 'liTxt');

    node.appendChild(textNode);

    var input = document.createElement("input");
    input.setAttribute("type", "hidden");
    input.setAttribute("name", "phoneId");

    if (id != null) {
        input.setAttribute("value", id);
    }

    node.appendChild(input);
    node.appendChild(span);

    return node;
}

function createTextFromListItem(liText) {
    return liText.substr(0, liText.indexOf('<'));
}

function addItem() {
    const addNameTxt = document.getElementById("add-name");
    const addAddressTxt = document.getElementById("add-address");
    const addDateOfBirthTxt = document.getElementById("add-dateOfBirth");

    const numbers = $('ul.listEdit>li').toArray();

    var jsonNumbers = [];

    for (var index = 0; index < numbers.length; ++index) {
        var txt = numbers[index].textContent;
        txt = txt.replace("x", "");

        var obj = { number: txt };
        jsonNumbers.push(obj);
    }

    const item = {
        name: addNameTxt.value.trim(),
        address: addAddressTxt.value.trim(),
        dateOfBirth: new Date(addDateOfBirthTxt.value.trim()),
        numbers: jsonNumbers
    };

    fetch(url,
        {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        }).then(response => response.json()).then(() => {
            getItems();
            _emptyAddForm();
    }).catch(error => console.error('Unable to add item', error));
}



