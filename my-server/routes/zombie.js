const express = require('express');
const router = express.Router();
const db = require('../db/db');


router.get('/', (req, res) => {
    db.query('SELECT * FROM zombie_db', (err, results) => {
        if (err) return res.status(500).json({ error: 'DB 조회 실패' });
        const key = "zombie";
        res.json({ [key]: results });
    });
});
module.exports = router