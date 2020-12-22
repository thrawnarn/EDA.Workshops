"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.apply = void 0;
var GameStatus;
(function (GameStatus) {
    GameStatus[GameStatus["None"] = 0] = "None";
    GameStatus[GameStatus["ReadyToStart"] = 10] = "ReadyToStart";
    GameStatus[GameStatus["Started"] = 20] = "Started";
    GameStatus[GameStatus["Ended"] = 50] = "Ended";
})(GameStatus || (GameStatus = {}));
exports.apply = (state, e) => {
    switch (e.type) {
        case 'gameCreated':
            return Object.assign(Object.assign({}, state), { status: GameStatus.ReadyToStart });
        default:
            return state;
    }
};
//# sourceMappingURL=game.js.map