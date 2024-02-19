from cryptography.hazmat.primitives.hashes import *
from cryptography.hazmat.primitives.hmac import *
from cryptography.hazmat.backends import default_backend

backend = default_backend()

def get_signature(h, text):
    h.update(text)
    return h.finalize()

key = b'key'
text = b'The quick brown fox jumps over the lazy dog'

hmacs = [
    ('HMAC_SHA256("$", "@"):', HMAC(key, SHA256())),
    ('HMAC_SHA3-256("$", "@"):', HMAC(key, SHA3_256())),
    ('HMAC_SHA512("$", "@"):', HMAC(key, SHA512())),
    ('HMAC_SHA3-512("$", "@"):', HMAC(key, SHA3_512())),
]

signature = ''

for (dname, h) in hmacs:
    print(dname.replace('$', key.decode('utf-8')).replace('@', text.decode('utf-8')))
    sig = get_signature(h, text)
    print(sig.hex())
    print()
    if dname == 'HMAC_SHA3-512("$", "@"):':
        signature = sig

h = HMAC(key, SHA3_512())
h.update(text)

try:
    h.verify(signature)
except:
    print(f"The message '{text.decode('utf-8')}' is not authentic")
else:
    print(f"The message '{text.decode('utf-8')}' is authentic")
