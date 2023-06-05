document.querySelectorAll('.add-to-cart').forEach(function (button) {
    button.addEventListener('click', function (e) {
        const productId = document.querySelector('#product-id').value;
        fetch('/cart/add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ productId: productId, quantity: 1 })
        })
            .then((response) => {
                if (response.status === 401) {
                    toastr.error('Трябва да влезете в акаунта си, за да добавите продукт в количката си');
                } else {
                    getUserCart();
                    toastr.success('Продуктът е добавен в количката');
                }
            })
            .catch(function (error) {
                console.warn('Error: ' + error?.message);
            });
    });
});

document.querySelectorAll('.removeButton')?.forEach(function (button) {
    button.addEventListener('click', function (e) {
        const productId = e.target.dataset.productId;
        fetch('/cart/remove/' + productId, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    return response.json().then(function (json) {
                        throw new Error(json.error);
                    });
                }
                else {
                    getUserCart();
                }

                return response;
            })
            .then(function () {
                toastr.success('Продуктът е премахнат от количката');
                e.target.parentElement.parentElement.remove();
            })
            .catch(function (error) {
                console.warn('Error: ' + error?.message);
            });
    });
});

document.querySelector('#clearButton')?.addEventListener('click', function () {
    fetch('/cart/clear', {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.error);
                });
            }

            return response;
        })
        .then(function () {
            location.reload();
        })
        .catch(function (error) {
            console.warn('Error: ' + error.message);
        });
});

function getUserCart() {
    fetch('/cart/get')
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.error)
                });
            }

            return response.json();
        })
        .then(function (cart) {
            const count = cart.items.reduce(function (total, item) {
                return total + item.quantity;
            }, 0);

            document.getElementById('cart-count').textContent = count;
        })
        .catch(function (error) {
            console.warn('Error: ' + error.message);
        });
}

getUserCart();