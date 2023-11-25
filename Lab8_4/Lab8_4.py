# Piotr Deda

from cryptography.hazmat.primitives.hashes import *
from cryptography.hazmat.backends import default_backend

backend = default_backend()

class MD4(HashAlgorithm):
    name = 'md4'
    digest_size = 16
    block_size = 64

texts = [
    bytes.fromhex('839c7a4d7a92cb5678a5d5b9eea5a7573c8a74deb366c3dc20a083b69f5d2a3bb3719dc69891e9f95e809fd7e8b23ba6318edd45e51fe39708bf9427e9c3e8b9'),
    bytes.fromhex('839c7a4d7a92cbd678a5d529eea5a7573c8a74deb366c3dc20a083b69f5d2a3bb3719dc69891e9f95e809fd7e8b23ba6318edc45e51fe39708bf9427e9c3e8b9'),
    bytes.fromhex('a6af943ce36f0cf4adcb12bef7f0dc1f526dd914bd3da3cafde14467ab129e640b4c41819915cb43db752155ae4b895fc71b9b0d384d06ef3118bbc643ae6384'),
    bytes.fromhex('a6af943ce36f0c74adcb122ef7f0dc1f526dd914bd3da3cafde14467ab129e640b4c41819915cb43db752155ae4b895fc71b9a0d384d06ef3118bbc643ae6384'),
    bytes.fromhex('76931fac9dab2b36c248b87d6ae33f9a62d7183a5d5789e4b2d6b441e2411dc709e111c7e1e7acb6f8cac0bb2fc4c8bc2ae3baaab9165cc458e199cb89f51b13'),
    bytes.fromhex('76931fac9dab2b36d248b87d6af33f9a62d7183a5d5789e4b2d6b441e2411dc709e111c7e1e7acb6f8cac0bb2fc4c8bc2ae3baaab9265cc458e199cb89f51b13'),
]

results = []

for m in texts:
    digest = Hash(MD4(), backend)
    digest.update(m)
    results.append(digest.finalize().hex())
    print('Digest is: ' + results[-1])

print()
print('m1 == m2? ' + str(results[0] == results[1]))
print('m3 == m4? ' + str(results[2] == results[3]))
print('m5 == m6? ' + str(results[4] == results[5]))
