// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//----------------------------------------------------------------------------
//Fixed Navigation
window.addEventListener("scroll", function () {
    var navbar = document.querySelector(".main-nav");

    if (window.scrollY > 50) {
        navbar.classList.add("fixed-top");
    } else {
        navbar.classList.remove("fixed-top");
    }
});
// Navigation
document.addEventListener("DOMContentLoaded", function () {
    const navLinks = document.querySelectorAll(".nav-link");

    // Xác định đường dẫn hiện tại
    const currentPath = window.location.pathname.toLowerCase();

    navLinks.forEach(link => {
        const linkPath = new URL(link.href, window.location.origin).pathname.toLowerCase();

        // Kiểm tra nếu trang hiện tại khớp với link thì thêm class "active"
        if (currentPath === linkPath) {
            link.classList.add("active");
        }

        // Hiệu ứng underline khi hover
        link.addEventListener("mouseenter", function () {
            this.classList.add("hover-effect");
        });

        link.addEventListener("mouseleave", function () {
            this.classList.remove("hover-effect");
        });
    });
});

//Section homgpage
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            window.scrollTo({
                top: target.offsetTop - 50,
                behavior: 'smooth'
            });
        }
    });
});

//Đánh giá
document.addEventListener("DOMContentLoaded", function () {
    const stars = document.querySelectorAll(".star");
    let selectedRating = 0;

    // Khi hover vào sao
    stars.forEach(star => {
        star.addEventListener("mouseover", function () {
            resetStars();
            highlightStars(this.dataset.value);
        });
        star.addEventListener("mouseout", function () {
            resetStars();
            highlightStars(selectedRating);
        });
        star.addEventListener("click", function () {
            selectedRating = this.dataset.value;
            highlightStars(selectedRating);
        });
    });

    function resetStars() {
        stars.forEach(star => star.classList.remove("active"));
    }

    function highlightStars(count) {
        for (let i = 0; i < count; i++) {
            stars[i].classList.add("active");
        }
    }

    // Xử lý khi nhấn nút gửi đánh giá
    document.getElementById("submitReview").addEventListener("click", function () {
        const feedback = document.getElementById("feedback").value.trim();

        if (selectedRating > 0 && feedback !== "") {
            // Hiển thị cửa sổ xác nhận thành công
            Swal.fire({
                icon: "success",
                title: "Đánh giá thành công!",
                text: "Cảm ơn bạn đã đóng góp ý kiến chúng tôi sẽ cố gắng hoàn thiện hơn ❤️",
                showConfirmButton: false,
                timer: 2500
            });
        } else {
            // Hiển thị toast cảnh báo
            Swal.fire({
                toast: true,
                position: "bottom-end",
                icon: "warning",
                title: "Vui lòng chọn số sao và nhập góp ý!",
                showConfirmButton: false,
                timer: 2000
            });
        }
    });
});

//Save Profile user & admin
document.getElementById("saveProfile").addEventListener("click", function () {
    let userRole = this.getAttribute("data-role"); // Lấy giá trị data-role ("admin" hoặc "user")

    // Hiển thị modal xác nhận đổi mật khẩu
    Swal.fire({
        title: "Xác nhận",
        text: "Bạn có chắc muốn đổi mật khẩu không?",
        icon: "warning",
        confirmButtonColor: "#3085d6",
        confirmButtonText: "Xác nhận",
    }).then((result) => {
        if (result.isConfirmed) {
            // Hiển thị thông báo thành công
            Swal.fire({
                title: "Thành công!",
                text: userRole === "admin" ? "Mật khẩu Admin đã được cập nhật." : "Mật khẩu Người dùng đã được cập nhật.",
                icon: "success",
                confirmButtonText: "OK"
            });

            // Ẩn modal chỉnh sửa
            var editModal = new bootstrap.Modal(document.getElementById("editProfileModal"));
            editModal.hide();
        }
    });
});

