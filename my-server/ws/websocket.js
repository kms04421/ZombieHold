const { WebSocketServer } = require('ws');
function initWebSocket(server) {
    const wss = new WebSocketServer({ server });
    console.log('WebSocket 서버 실행됨');

    const zombies = {};
    const players = {};

    const { v4: uuidv4 } = require('uuid');

    wss.on('connection', (ws) => {
        const playerId = uuidv4();

        const newPlayer = { id: playerId, position: { x: 0, y: 0, z: 0 } };
        players[playerId] = newPlayer;

        ws.send(JSON.stringify({ type: "AssingID", data: { id: playerId } })); //본인 id전송
        // 1) 기존 플레이어에게 새 플레이어 등장 메시지
        wss.clients.forEach(client => {
            if (client.readyState === 1 && client !== ws) {
                client.send(JSON.stringify({ type: "NewPlayer", data: newPlayer }));
            }
        });

        // 2) 새 플레이어에게 기존 플레이어 정보 전송
        const existingPlayers = Object.values(players)
            .filter(p => p.id !== playerId);  // 자기 자신 제외
        ws.send(JSON.stringify({ type: "existingPlayers", data: existingPlayers }));

        // 3) 새 플레이어에게 자기 자신 정보 전송 (생성용)
        ws.send(JSON.stringify({ type: "NewPlayer", data: newPlayer }));


        ws.on('message', (message) => {
            const msg = JSON.parse(message);
            const data = msg.data;
            switch (msg.type) {
            
                    //여기서부터 좀비
                case 'registerZombie':
                    zombies[data.id] = {
                        hp: data.hp,
                        template: data.template, // 나중에 템플릿/스킬 정보도 저장 가능
                        position: data.position || { x: 0, y: 0, z: 0 },
                    };
                    console.log(`Zombie registered: ID=${data.id}, HP=${data.hp}`);
                    break;

                case 'damageZombie':
                    const z = zombies[data.id];
                    if (z) {
                        z.hp -= data.damage;
                        const isDead = z.hp <= 0;

                        const hitMsg = JSON.stringify({
                            type: 'zombieHit',
                            id: data.id,
                            hp: z.hp > 0 ? z.hp : 0,
                            dead: isDead
                        });

                        // 모든 클라이언트로 전송
                        wss.clients.forEach(client => {
                            if (client.readyState === 1) client.send(hitMsg);
                        });

                        if (isDead) delete zombies[data.id];
                    }
                    break;            

                case 'zombieUpdate':
                    break;
                    // 여기서 부터 플레이어
                case 'registerPlayer':
                    players[data.id] = {
                        id : data.id,
                        hp: data.hp,
                        maxHP: data.maxHP,
                        position: { x: 0, y: 0, z: 0 }
                    };
                    console.log(`Player registered: ID=${data.id}, MaxHP=${data.maxHP}`);
                    break;

                case 'damagePlayer':
                    const p = players[data.id];
                    if (p) {
                        p.hp -= data.damage;
                        const isDead = p.hp <= 0;

                        const hitMsg = JSON.stringify({
                            type: 'playerHit',
                            id: data.id,
                            hp: p.hp > 0 ? p.hp : 0
                        });

                        wss.clients.forEach(client => {
                            if (client.readyState === 1) client.send(hitMsg);
                        });
                        console.log(`Player registered: ID=${data.id}, currentHP=${p.hp}`);
                    }
                    break;           
                case 'playerUpdate':
  
                    if (players[data.id]) {
                        players[data.id].position = {
                            x: data.position.x,
                            y: data.position.y,
                            z: data.position.z
                        };
                    }

                    const playerPosMsg = JSON.stringify({
                        type: 'playerPosUpdate',
                        data: {
                            id: data.id,
                            position: {
                                x: players[data.id].position.x,
                                y: players[data.id].position.y,
                                z: players[data.id].position.z
                            }
                        
                        }
                  
                    });
                    console.log('playerUpdate:', data.id, players[data.id].x, players[data.id].position.y, data.z, players[data.id]);
                    wss.clients.forEach(client => {
                        if (client.readyState === 1 && client !== ws) client.send(playerPosMsg);
                    });
                    break;

                case 'nightStart':
                    const zombieCount = Math.floor(Math.random() * 5) + 5;
                    const spawnMsg = JSON.stringify({ type: 'spawnZombie', count: zombieCount });

                    wss.clients.forEach(client => {
                        if (client.readyState === 1) client.send(spawnMsg);
                    });
                    break;
            }
        });

        ws.on('close', () => console.log('클라이언트 연결 종료'));
    });

/*    // 100ms 마다 상태 브로드캐스트
    setInterval(() => {
        const state = { players, zombies };
        const msg = JSON.stringify({ type: 'stateUpdate', data: state });

        wss.clients.forEach(client => {
            if (client.readyState === 1) client.send(msg);
        });
    }, 100);*/
}

module.exports = initWebSocket;