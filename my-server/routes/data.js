const express = require('express');
const router = express.Router();
const db = require('../db/db'); // DB 모듈 불러오기

router.get('/:type', async (req, res) => {
    const { type } = req.params;
    let tableName = '';

    if (type === 'zombie') tableName = 'zombie_db';
    else if (type === 'item') tableName = 'item_db';
    else if (type === 'ability') tableName = 'ability_db';
    else return res.status(400).send('유효하지 않은 이름');

    try {
        const [results] = await db.query(`SELECT * FROM ${tableName}`);
        res.json({ [type]: results });
    } catch (err) {
        console.error(err);
        res.status(500).send(`${tableName} 조회 실패`);
    }
});

router.post('/data', async (req, res) => {
    const { id } = req.body;
    try {
        const [results] = await db.query('SELECT * FROM item_db WHERE id = ?', [id]);
        res.json(results);
    } catch (err) {
        console.error(err);
        res.status(500).send('DB 조회 실패');
    }
});
module.exports = router;