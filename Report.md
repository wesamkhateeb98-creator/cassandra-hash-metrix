## Iteration 1/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 4.16 | 856 | 769 | 2523 | 874 | 1144 | 5 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1330 | 47.89 | 54.65 | 1092 | 880 | 3812 | 1162 | 861| 10 |
| Δ (2 − 1) | | | +32 | +986 | +47.89 | +50.49 | +237 | +111 | +1289 | +288 | -283 | +5 |

## Iteration 2/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 3.84 | 835 | 688 | 2919 | 851 | 1175 | 8 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1330 | 27.56 | 32.72 | 572 | 522 | 1875 | 612 | 1633 | 9 |
| Δ (2 − 1) | | | +32 | +986 | +27.56 | +28.88 | -263 | -166 | -1044 | -239 | +459 | +1 |

## Iteration 3/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 2.46 | 479 | 398 | 1516 | 487 | 2055 | 8 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1330 | 25.95 | 30.47 | 551 | 469 | 2024 | 588 | 1700 | 9 |
| Δ (2 − 1) | | | +32 | +986 | +25.95 | +28.01 | +72 | +71 | +507 | +102 | -355 | +1 |

## Iteration 4/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 2.83 | 455 | 392 | 1747 | 464 | 2157 | 8 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1330 | 27.17 | 32.14 | 632 | 482 | 3016 | 671 | 1491 | 9 |
| Δ (2 − 1) | | | +32 | +986 | +27.17 | +29.31 | +178 | +90 | +1269 | +207 | -666 | +1 |

## Iteration 5/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 3.33 | 605 | 480 | 2461 | 615 | 1626 | 8 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1330 | 26.37 | 30.88 | 555 | 459 | 1809 | 592 | 1689 | 9 |
| Δ (2 − 1) | | | +32 | +986 | +26.37 | +27.55 | -50 | -22 | -652 | -23 | +63 | +1 |

## Iteration 6/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 2.21 | 443 | 399 | 1354 | 451 | 2219 | 8 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1331 | 27.13 | 31.45 | 533 | 460 | 1758 | 570 | 1753 | 9 |
| Δ (2 − 1) | | | +32 | +987 | +27.13 | +29.24 | +90 | +61 | +405 | +120 | -466 | +1 |

## Iteration 7/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 2.77 | 613 | 441 | 3015 | 622 | 1607 | 8 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1330 | 37.85 | 43.98 | 782 | 659 | 2795 | 833 | 1200 | 9 |
| Δ (2 − 1) | | | +32 | +986 | +37.85 | +41.21 | +168 | +218 | -220 | +211 | -408 | +1 |

## Iteration 8/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 2.98 | 480 | 402 | 2130 | 488 | 2048 | 9 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1330 | 25.88 | 30.88 | 522 | 458 | 1647 | 559 | 1790 | 8 |
| Δ (2 − 1) | | | +32 | +986 | +25.88 | +27.90 | +42 | +56 | -483 | +70 | -258 | -1 |

## Iteration 9/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 465 | 344 | 0.00 | 2.20 | 473 | 390 | 1407 | 480 | 2085 | 7 |
| 2 with hash | proofs_by_session | 1000 | 497 | 1331 | 27.87 | 32.30 | 502 | 437 | 1560 | 540 | 1853 | 9 |
| Δ (2 − 1) | | | +32 | +987 | +27.87 | +30.10 | +29 | +47 | +153 | +60 | -231 | +2 |

## Iteration 10/10: 1000 + 1000 = 2000 inserts

| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 no hash | proofs_by_session_no_hash | 1000 | 471 | 344 | 0.00 | 3.55 | 832 | 528 | 4846 | 841 | 1188 | 8 |
| 2 with hash | proofs_by_session | 1000 | 503 | 1330 | 35.90 | 42.65 | 953 | 731 | 3487 | 1004 | 996 | 9 |
| Δ (2 − 1) | | | +32 | +986 | +35.90 | +39.10 | +121 | +203 | -1358 | +162 | -192 | +1 |