document.getElementById('searchBtn').addEventListener('click', () => {
    window.location.href = `/Search/${document.getElementById('searchInput').value}/1`;
});