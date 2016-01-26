angular.module('CartoonsApp')
.controller('CartoonController', function () {
    this.cartoons = [
        { Id: 1, Name: "Duck Tales", Species: ["Duck"] },
        { Id: 2, Name: "Thundercats", Species: ["WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal","Toad"] },
        { Id: 3, Name: "Pirates of Dark Water", Species: ["Human", "Monkeybird", "Pirate", "Dragon"] },
        { Id: 4, Name: "Poddington Peas", Species: ["Pea"] },
        { Id: 5, Name: "Peter Pan and the Pirates", Species: ["Human", "Pirate"] },
        { Id: 6, Name: "Rupert", Species: ["Bear"] },
        { Id: 7, Name: "Winnie the Pooh", Species: ["Bear"] },
        { Id: 8, Name: "Tailspin", Species: ["Bear"] },
        { Id: 9, Name: "Sharkey and George", Species: ["Fish","Shark"] },
        { Id: 10, Name: "Teenage Mutant Hero Turtles", Species: ["Turtle", "Rat", "Human"] },
        { Id: 11, Name: "Farthing Wood", Species: ["Fox", "Badger", "Toad", "Weasel","Owl","Newt", "Mole"] },
        { Id: 12, Name: "Dungeons and Dragons", Species: ["Human", "Wizard", "Dragon", "Unicorn"] }
    ];
});