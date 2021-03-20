# Deskripsi
Algoritma yang diimplementasikan adalah algoritma BFS dan DFS. 
## BFS
BFS (Breadth First Search) adalah algoritma pencarian dalam suatu graf yang dalam program ini memiliki langkah:
1. Pilih suatu node awal
2. Masukkan semua node tetangga dari node terpilih ke queue node aktif secara terurut membesar dari abjad, jika node sudah pernah ada di queue jangan dimasukkan
3. Pilih node (dequeue) dari queue
4. Ulangi langkah 2-3 sampai node yang terpilih adalah node yang dicari atau tidak ada node lagi di graf yang belum terpilih (tidak ada yang bisa di-dequeue)
## DFS
DFS( Depth First Search) adalah algoritma pencarian dalam suatu graf yang dalam program ini memiliki langkah:
1. Dipanggil fungsi DFS dengan node pertama di graph sebagai node terpilih.
2. Untuk semua node tetangga dari node terpilih yang belum pernah dipilih, dipanggil lagi fungsi DFS secara rekursi dengan node tersebut sebagai node terpilih
3. Rekursi dilakukan sampai node yang dipilih adalah node target.
# Requirements

# How to Run

# Author
- Alexander 13519090
- Girvin Junod 13519096
- Josep Marcello 13519164
