let siralananSutun = -1;
let mevcutYon = "asc";
function sirala(n, tabloId = "tablo") {
    const table = document.getElementById(tabloId);
    if (!table) return;

    const tbody = table.querySelector("tbody");
    const rows = Array.from(tbody.querySelectorAll("tr"));
    const basliklar = table.querySelectorAll("thead th");

    if (siralananSutun === n) {
        mevcutYon = (mevcutYon === "asc") ? "desc" : "asc";
    } else {
        siralananSutun = n;
        mevcutYon = "asc";
    }

    rows.sort((a, b) => {
        const xCell = a.getElementsByTagName("td")[n];
        const yCell = b.getElementsByTagName("td")[n];

        if (!xCell || !yCell) return 0;

        const xText = xCell.textContent.trim();
        const yText = yCell.textContent.trim();

        const xNum = parseFloat(xText);
        const yNum = parseFloat(yText);
        const ikisiDeSayi = !isNaN(xNum) && !isNaN(yNum);

        let sonuc = ikisiDeSayi ? (xNum - yNum) : xText.localeCompare(yText, 'tr');
        return mevcutYon === "asc" ? sonuc : -sonuc;
    });

    rows.forEach(row => tbody.appendChild(row));

    basliklar.forEach((th, index) => {
        const ikon = th.querySelector(".sirala-ikon");
        if (!ikon) return;
        ikon.textContent = (index === n) ? (mevcutYon === "asc" ? " ▲" : " ▼") : " ↕️";
    });
}