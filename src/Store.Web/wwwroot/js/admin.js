document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll('.delete-product');

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            const productId = this.getAttribute('data-product-id');

            if (confirm('Сигурни ли сте, че искате да изтриете продукта?')) {
                fetch(`/admin/products/delete/${productId}`, {
                    method: 'DELETE',
                })
                    .then(response => {
                        if (response.ok) {
                            toastr.success('Продуктът е изтрит успешно');
                            e.target.parentElement.parentElement.remove();
                        } else {
                            toastr.error('Възниква проблем с изтриването на продукта');
                        }
                    })
                    .catch(error => {
                        console.warn(error?.message);
                    });
            }
        });
    });
});