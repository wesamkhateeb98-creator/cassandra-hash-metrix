
| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 446 | 344 | 0.00 | 7.05 | 1755 | 1284 | 6616 | 1778 | 562 | 5 |
| 2 with hash | proofs_by_session | 1000 | 478 | 1306 | 55.15 | 62.95 | 1376 | 1030 | 5506 | 1449 | 690 | 9 |
| Δ (2 − 1) | | | +32 | +962 | +55.15 | +55.90 | -379 | -254 | -1110 | -329 | +128 | +4 |

---

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 446 | 344 | 0.00 | 7.05 | 1755 | 1284 | 6616 | 1778 | 562 | 5 |
| 2 with hash | proofs_by_session | 1000 | 478 | 1306 | 55.15 | 62.95 | 1376 | 1030 | 5506 | 1449 | 690 | 9 |
| Δ (2 − 1) | | | +32 | +962 | +55.15 | +55.90 | -379 | -254 | -1110 | -329 | +128 | +4 |


---
| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 446 | 344 | 0.00 | 10.16 | 1327 | 1004 | 6680 | 1357 | 737 | 4 |
| 2 with hash | proofs_by_session | 1000 | 478 | 1306 | 59.98 | 71.25 | 1334 | 851 | 5587 | 1421 | 704| 10 |
| Δ (2 − 1) | | | +32 | +962 | +59.98 | +61.09 | +7 | -153 | -1093 | +64 | -33 | +6 |

---
| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 446 | 344 | 0.00 | 5.07 | 1005 | 833 | 3922 | 1024 | 976 | 5 |
| 2 with hash | proofs_by_session | 1000 | 478 | 1307 | 48.02 | 55.06 | 847 | 726 | 2738 | 912 | 1096 | 9 |
| Δ (2 − 1) | | | +32 | +962 | +48.02 | +49.99 | -158 | -106 | -1184 | -112 | +120 | +4 |