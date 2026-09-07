function aramaYap(inputId = "arama-input", tabloSecici = "table tbody") {
    const input = document.getElementById(inputId);
    const tablo = document.querySelector(tabloSecici);

    if (!input || !tablo) return;

    const arananKelime = input.value.toLocaleLowerCase('tr').trim();
    const satirlar = tablo.getElementsByTagName("tr");

    for (let i = 0; i < satirlar.length; i++) {
        const hucreler = satirlar[i].getElementsByTagName("td");
        if (hucreler.length === 0) continue;

        let eslesmeVar = false;

        //satırdaki tüm hücreleri dinamik olarak kontrol eder
        for (let j = 0; j < hucreler.length; j++) {
            const hucreMetni = hucreler[j].textContent.toLocaleLowerCase('tr');
            if (hucreMetni.includes(arananKelime)) {
                eslesmeVar = true;
                break;
            }
        }
        satirlar[i].style.display = eslesmeVar ? "" : "none";
    }
}

//input ve temizleme butonu dinleyicisi
document.addEventListener("DOMContentLoaded", function () {
    const input = document.getElementById("arama-input");
    const temizleBtn = document.getElementById("temizle-btn");

    if (!input) return;

    //yazı yazıldıkça temizle butonunu göster/gizle ve canlı filtrele
    input.addEventListener("input", function () {
        if (temizleBtn) {
            temizleBtn.style.display = input.value.trim().length > 0 ? "block" : "none";
        }
    });

    //tıklayınca arama
    input.addEventListener("keydown", function (e) {
        if (e.key === "Enter") {
            aramaYap();
        }
    });

    //çarpı butonuna basınca temizler
    if (temizleBtn) {
        temizleBtn.addEventListener("click", function () {
            input.value = "";
            temizleBtn.style.display = "none";
            input.focus();
            aramaYap();
        });
    }
});