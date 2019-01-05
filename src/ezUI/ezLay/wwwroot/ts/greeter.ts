﻿class Student {
    fullName: string;
    constructor(public firstName:string, public middleInitial:number, public lastName) {
        this.fullName = firstName + " " + middleInitial + " " + lastName;
    }
}

interface Person {
    firstName: string;
    lastName: string;
}

function greeter(person: Person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}

let user = new Student("Jane", 1, "User");

document.body.innerHTML = greeter(user);
