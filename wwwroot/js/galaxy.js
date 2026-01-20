(function () {
  const overlay = document.getElementById("planetOverlay");
  const closeBtn = document.getElementById("ovClose");

  const ovName = document.getElementById("ovName");
  const ovLevel = document.getElementById("ovLevel");
  const ovPps = document.getElementById("ovPps");

  const ovLockedRow = document.getElementById("ovLockedRow");
  const ovUpgradeRow = document.getElementById("ovUpgradeRow");
  const ovUnlockPrice = document.getElementById("ovUnlockPrice");

  const ovC1 = document.getElementById("ovC1");
  const ovC5 = document.getElementById("ovC5");
  const ovC10 = document.getElementById("ovC10");

  const buyPlanetId = document.getElementById("ovBuyPlanetId");
  const uPid1 = document.getElementById("ovUPlanetId1");
  const uPid5 = document.getElementById("ovUPlanetId5");
  const uPid10 = document.getElementById("ovUPlanetId10");

  function openOverlayFromPlanet(el) {
    const id = el.dataset.planetId;
    const name = el.dataset.name;
    const unlocked =
      el.dataset.unlocked === "True" || el.dataset.unlocked === "true";

    ovName.textContent = name;
    ovLevel.textContent = el.dataset.level || "0";
    ovPps.textContent = el.dataset.pps || "0";

    if (!unlocked) {
      ovLockedRow.classList.remove("hidden");
      ovUpgradeRow.classList.add("hidden");
      ovUnlockPrice.textContent = el.dataset.unlockprice || "0";
      buyPlanetId.value = id;
    } else {
      ovLockedRow.classList.add("hidden");
      ovUpgradeRow.classList.remove("hidden");

      ovC1.textContent = el.dataset.cost1 || "0";
      ovC5.textContent = el.dataset.cost5 || "0";
      ovC10.textContent = el.dataset.cost10 || "0";

      uPid1.value = id;
      uPid5.value = id;
      uPid10.value = id;
    }

    overlay.classList.remove("hidden");
  }

  document.querySelectorAll(".planet").forEach((p) => {
    p.addEventListener("click", () => openOverlayFromPlanet(p));
  });

  closeBtn.addEventListener("click", () => overlay.classList.add("hidden"));
  overlay.addEventListener("click", (e) => {
    if (e.target === overlay) overlay.classList.add("hidden");
  });
})();
